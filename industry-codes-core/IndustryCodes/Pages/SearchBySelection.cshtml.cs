//
//  SearchBySelection.cshtml.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using IndustryCodes.Models;
using IndustryCodes.ViewModels;
using IndustryCodes.Utility;
using Microsoft.EntityFrameworkCore;

namespace IndustryCodes.Pages
{
    public class SearchBySelectionModel : PageModel
    {
        private readonly IndustryCodesContext context;

        private const int defaultSelectionValue = 0;
        private const string defaultSelectionText = "===  select one  ===";

        private IEnumerable<IndustrySector> industrySectors;
        private int selectedIndustrySector;
        private int? previousSelectedIndustrySector;

        private IEnumerable<IndustrySubsector> industrySubsectors;
        private int selectedIndustrySubsector;
        private int? previousSelectedIndustrySubsector;

        private IEnumerable<NAICSDescription> naicsDescriptions;
        private int selectedNAICSDescription;
        private int? previousSelectedNAICSDescription;

        private IEnumerable<SICDescription> sicDescriptions;
        private int selectedSICDescription;

        public SearchBySelectionModel(IndustryCodesContext industryCodesContext)
        {
            context = industryCodesContext ?? throw new ArgumentNullException(nameof(industryCodesContext));
        }

        public async Task<IActionResult> OnGet()
        {
            ViewModel = new SearchBySelectionViewModel();

            selectedIndustrySector = 0;
            selectedIndustrySubsector = 0;
            selectedNAICSDescription = 0;
            selectedSICDescription = 0;

            industrySectors = await GetIndustrySectors();

            ViewModel.IndustrySectors = new SelectList(industrySectors, "Code", "Description");
            ViewModel.SelectedIndustrySector = 0;

            ViewModel.IndustrySubsectors = new SelectList(new Collection<IndustrySubsector>(), "Code", "Description");
            ViewModel.SelectedIndustrySubsector = 0;

            ViewModel.NAICSDescriptions = new SelectList(new Collection<NAICSDescription>(), "Code", "Description");
            ViewModel.SelectedNAICSDescription = 0;

            sicDescriptions = await GetSICDescriptions(null);

            ViewModel.SICDescriptions = new SelectList(new Collection<SICDescription>(), "Code", "Description");
            ViewModel.SelectedSICDescription = 0;

            SetSessionVariables();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection collection)
        {
            if (FormCollectionAssistant.IsFormButtonSelected("ClearButton", "Clear", collection))
            {
                return RedirectToPage("/SearchBySelection");
            }

            GetSessionVariables();

            industrySectors = await GetIndustrySectors();
            selectedIndustrySector = FormCollectionAssistant.GetFormNumberValue("ViewModel.SelectedIndustrySector", collection);

            ViewModel.IndustrySectors = new SelectList(industrySectors, "Code", "Description");
            ViewModel.SelectedIndustrySector = selectedIndustrySector;

            if (selectedIndustrySector != previousSelectedIndustrySector)
            {
                industrySubsectors = await GetIndustrySubsectors();

                ViewModel.IndustrySubsectors = new SelectList(industrySubsectors, "Code", "Description");
                ViewModel.SelectedIndustrySubsector = selectedIndustrySubsector;

                naicsDescriptions = await GetNAICSDescriptions();

                ViewModel.NAICSDescriptions = new SelectList(naicsDescriptions, "Code", "Description");
                ViewModel.SelectedNAICSDescription = selectedNAICSDescription;

                sicDescriptions = await GetSICDescriptions(null);

                ViewModel.SICDescriptions = new SelectList(sicDescriptions, "Code", "Description");
                ViewModel.SelectedSICDescription = selectedSICDescription;

                SetSessionVariables();

                return Page();
            }

            industrySubsectors = await GetIndustrySubsectors();
            selectedIndustrySubsector = FormCollectionAssistant.GetFormNumberValue("ViewModel.SelectedIndustrySubsector", collection);

            ViewModel.IndustrySubsectors = new SelectList(industrySubsectors, "Code", "Description");
            ViewModel.SelectedIndustrySubsector = selectedIndustrySubsector;

            if (selectedIndustrySubsector != previousSelectedIndustrySubsector)
            {
                naicsDescriptions = await GetNAICSDescriptions();

                ViewModel.NAICSDescriptions = new SelectList(naicsDescriptions, "Code", "Description");
                ViewModel.SelectedNAICSDescription = selectedNAICSDescription;

                sicDescriptions = await GetSICDescriptions(null);

                ViewModel.SICDescriptions = new SelectList(sicDescriptions, "Code", "Description");
                ViewModel.SelectedSICDescription = selectedSICDescription;

                SetSessionVariables();

                return Page();
            }

            naicsDescriptions = await GetNAICSDescriptions();
            selectedNAICSDescription = FormCollectionAssistant.GetFormNumberValue("ViewModel.SelectedNAICSDescription", collection);

            ViewModel.NAICSDescriptions = new SelectList(naicsDescriptions, "Code", "Description");
            ViewModel.SelectedNAICSDescription = selectedNAICSDescription;

            if (selectedNAICSDescription != previousSelectedNAICSDescription)
            {
                sicDescriptions = await GetSICDescriptions(null);

                ViewModel.SICDescriptions = new SelectList(sicDescriptions, "Code", "Description");
                ViewModel.SelectedSICDescription = selectedSICDescription;

                SetSessionVariables();

                return Page();
            }

            sicDescriptions = await GetSICDescriptions(null);
            selectedSICDescription = FormCollectionAssistant.GetFormNumberValue("ViewModel.SelectedSICDescription", collection);

            ViewModel.SICDescriptions = new SelectList(sicDescriptions, "Code", "Description");
            ViewModel.SelectedSICDescription = selectedSICDescription;

            if (selectedNAICSDescription > 0 || selectedSICDescription > 0)
            {
                ViewModel.NAICSCode = string.Format(CultureInfo.CurrentCulture, "{000000}", selectedNAICSDescription);
                ViewModel.NAICSDescription = await GetNAICSDescription(ViewModel.NAICSCode);
                ViewModel.SICCode = string.Format(CultureInfo.CurrentCulture, "{0000}", selectedSICDescription);
                ViewModel.SICDescription = await GetSICDescription(ViewModel.NAICSCode);
            }

            SetSessionVariables();

            return Page();
        }

        [BindProperty]
        public SearchBySelectionViewModel ViewModel { get; set; }

        private void GetSessionVariables()
        {
            previousSelectedIndustrySector = HttpContext.Session.GetInt32("SelectedIndustrySector");
            previousSelectedIndustrySubsector = HttpContext.Session.GetInt32("SelectedIndustrySubsector");
            previousSelectedNAICSDescription = HttpContext.Session.GetInt32("SelectedNAICSDescription");
        }

        private void SetSessionVariables()
        {
            HttpContext.Session.SetInt32("SelectedIndustrySector", selectedIndustrySector);
            HttpContext.Session.SetInt32("SelectedIndustrySubsector", selectedIndustrySubsector);
            HttpContext.Session.SetInt32("SelectedNAICSDescription", selectedNAICSDescription);
        }

        private async Task<string> GetIndustrySector()
        {
            var results = await context.ClassificationCodes
                                       .Select(c => c.IndustrySector)
                                       .Distinct()
                                       .OrderBy(description => description)
                                       .ToListAsync();

            var index = 0;
            foreach (var result in results)
            {
                if (selectedIndustrySector == ++index)
                {
                    return result;
                }
            }
            return null;
        }

        private async Task<IEnumerable<IndustrySector>> GetIndustrySectors()
        {
            var list = new Collection<IndustrySector>
            {
                new IndustrySector { Code = defaultSelectionValue, Description = defaultSelectionText }
            };

            var results = await context.ClassificationCodes
                                       .Select(c => c.IndustrySector)
                                       .Distinct()
                                       .OrderBy(description => description)
                                       .ToListAsync();

            selectedIndustrySector = 0;

            var index = 0;
            foreach (var result in results)
            {
                list.Add(new IndustrySector { Code = ++index, Description = result });
            }
            return list;
        }

        private async Task<string> GetIndustrySubsector()
        {
            var industrySector = await GetIndustrySector();

            var results = await context.ClassificationCodes
                                       .Where(c => c.IndustrySector == industrySector)
                                       .Select(c => c.IndustrySubsector)
                                       .Distinct()
                                       .OrderBy(description => description)
                                       .ToListAsync();

            var index = 0;
            return results.FirstOrDefault(description => selectedIndustrySubsector == ++index);
        }

        private async Task<IEnumerable<IndustrySubsector>> GetIndustrySubsectors()
        {
            var list = new Collection<IndustrySubsector>();

            if (selectedIndustrySector < 1)
            {
                return list;
            }

            list.Add(new IndustrySubsector { Code = defaultSelectionValue, Description = defaultSelectionText });

            var industrySector = await GetIndustrySector();

            var results = await context.ClassificationCodes
                                       .Where(c => c.IndustrySector == industrySector)
                                       .Select(c => c.IndustrySubsector)
                                       .Distinct()
                                       .OrderBy(description => description)
                                       .ToListAsync();

            var index = 0;
            foreach (var result in results)
            {
                list.Add(new IndustrySubsector { Code = ++index, Description = result });
            }
            return list;
        }

        private async Task<string> GetNAICSDescription(string naicsCode)
        {
            if (string.IsNullOrEmpty(naicsCode) && selectedNAICSDescription <= 0)
            {
                return null;
            }

            var naicsCodeOrIndex = !string.IsNullOrEmpty(naicsCode) ? Assistant.GetNumberValue(naicsCode) : selectedNAICSDescription;

            var results = context.ClassificationCodes
                                 .Where(c => c.NorthAmericanCode == naicsCodeOrIndex)
                                 .Select(c => c.NorthAmericanDescription)
                                 .Distinct();

            return await results.CountAsync() == 1 ? results.Single() : null;
        }

        private async Task<IEnumerable<NAICSDescription>> GetNAICSDescriptions()
        {
            var list = new Collection<NAICSDescription>();

            if (selectedIndustrySubsector < 1)
            {
                return list;
            }

            list.Add(new NAICSDescription { Code = defaultSelectionValue, Description = defaultSelectionText });

            var industrySubsector = await GetIndustrySubsector();

            var results = await context.ClassificationCodes
                                       .Where(c => c.IndustrySubsector == industrySubsector)
                                       .Select(c => new { code = c.NorthAmericanCode, description = c.NorthAmericanDescription })
                                       .Distinct()
                                       .ToListAsync();

            foreach (var result in results)
            {
                if (result.code == null || result.description == null)
                {
                    continue;
                }

                var index = Convert.ToInt32(result.code, CultureInfo.CurrentCulture);
                var element = result.code + " - " + result.description;

                list.Add(new NAICSDescription { Code = index, Description = element });
            }
            return list;
        }

        private async Task<string> GetSICDescription(string naicsCodeParameter)
        {
            if (selectedSICDescription < 1)
            {
                return null;
            }

            var naicsCode = Assistant.GetNumberValue(naicsCodeParameter);

            var sicCode = selectedSICDescription;

            var results = context.ClassificationCodes
                                 .Where(c => c.NorthAmericanCode == naicsCode && c.StandardCode == sicCode)
                                 .Select(c => new { description = c.StandardDescription });

            return await results.CountAsync() == 1 ? results.Single().description : null;
        }

        private async Task<IEnumerable<SICDescription>> GetSICDescriptions(string naicsCode)
        {
            var list = new Collection<SICDescription>();

            if (string.IsNullOrEmpty(naicsCode) && selectedNAICSDescription <= 0)
            {
                return list;
            }

            var naicsCodeOrIndex = !string.IsNullOrEmpty(naicsCode) ? Assistant.GetNumberValue(naicsCode) : selectedNAICSDescription;

            list.Add(new SICDescription { Code = defaultSelectionValue, Description = defaultSelectionText });

            var results = await context.ClassificationCodes
                                       .Where(c => c.NorthAmericanCode == naicsCodeOrIndex)
                                       .Select(c => new { code = c.StandardCode, description = c.StandardDescription })
                                       .ToListAsync();

            foreach (var result in results)
            {
                if (result.code == null || result.description == null)
                {
                    continue;
                }

                var index = Convert.ToInt32(result.code, CultureInfo.CurrentCulture);
                var element = result.code + " - " + result.description;

                list.Add(new SICDescription { Code = index, Description = element });
            }
            return list;
        }
    }
}
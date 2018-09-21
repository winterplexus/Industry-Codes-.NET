//
//  SearchByCode.cshtml.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-18
//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using IndustryCodes.Models;
using IndustryCodes.Utility;
using IndustryCodes.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IndustryCodes.Pages
{
    public class SearchByCodeModel : PageModel
    {
        private readonly IndustryCodesContext context;

        private const int defaultSelectionValue = 0;
        private const string defaultSelectionText = "===  select one  ===";

        private IEnumerable<SICDescription> sicDescriptions;
        private int selectedSICDescription;

        public SearchByCodeModel(IndustryCodesContext industryCodesContext)
        {
            context = industryCodesContext ?? throw new ArgumentNullException(nameof(industryCodesContext));
        }

        public IActionResult OnGet()
        {
            ViewModel = new SearchByCodeViewModel();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection collection)
        {
            if (FormCollectionAssistant.IsFormButtonSelected("ClearButton", "Clear", collection))
            {
                return RedirectToPage("/SearchByCode");
            }

            ViewModel.NAICSCode = FormCollectionAssistant.GetFormStringValue("ViewModel.NAICSCode", collection);
            ViewModel.NAICSDescription = FormCollectionAssistant.GetFormStringValue("ViewModel.NAICSDescription", collection);

            if (FormCollectionAssistant.IsFormButtonSelected("SearchButton", "Search", collection))
            {
                ViewModel.NAICSDescription = await GetNAICSDescription(ViewModel.NAICSCode);
                if (ViewModel.NAICSDescription == null)
                {
                    ModelState.AddModelError("ErrorMessage", string.Format(CultureInfo.CurrentCulture, "Unable to locate description for given NAICS code: {0}", ViewModel.NAICSCode));

                    ViewModel.NAICSCode = null;

                    return Page();
                }
            }

            sicDescriptions = await GetSICDescriptions(ViewModel.NAICSCode);
            selectedSICDescription = FormCollectionAssistant.GetFormNumberValue("ViewModel.SelectedSICDescription", collection);

            if (sicDescriptions != null)
            {
                ViewModel.SICDescriptions = new SelectList(sicDescriptions, "Code", "Description");
            }

            if (selectedSICDescription > 0)
            {
                ViewModel.SelectedSICDescription = selectedSICDescription;
            }

            if (selectedSICDescription > 0)
            {
                ViewModel.SICCode = string.Format(CultureInfo.CurrentCulture, "{0000}", selectedSICDescription);
            }

            if (selectedSICDescription > 0)
            {
                ViewModel.SICDescription = GetSICDescription();
            }

            return Page();
        }

        [BindProperty]
        public SearchByCodeViewModel ViewModel { get; set; }

        private async Task<string> GetNAICSDescription(string naicsCodeParameter)
        {
            if (string.IsNullOrEmpty(naicsCodeParameter))
            {
                return null;
            }

            var naicsCode = Assistant.GetNumberValue(naicsCodeParameter);

            var results = await context.ClassificationCodes
                                       .Where(c => c.NorthAmericanCode == naicsCode)
                                       .Select(c => c.NorthAmericanDescription)
                                       .Distinct()
                                       .ToListAsync();

            return results.Count() == 1 ? results.Single() : null;
        }

        private string GetSICDescription()
        {
            return sicDescriptions?.Where(s => s.Code == selectedSICDescription).Select(s => s.Description).FirstOrDefault();
        }

        private async Task<IEnumerable<SICDescription>> GetSICDescriptions(string naicsCodeParameter)
        {
            var list = new Collection<SICDescription>();

            if (string.IsNullOrEmpty(naicsCodeParameter))
            {
                return list;
            }

            var naicsCode = Assistant.GetNumberValue(naicsCodeParameter);

            list.Add(new SICDescription { Code = defaultSelectionValue, Description = defaultSelectionText });

            var results = await context.ClassificationCodes
                                       .Where(c => c.NorthAmericanCode == naicsCode)
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
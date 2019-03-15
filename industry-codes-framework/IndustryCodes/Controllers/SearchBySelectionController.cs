//
//  SearchBySelectionController.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-2019
//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using IndustryCodes.Database.Entities.Models;
using IndustryCodes.ViewModels;
using IndustryCodes.Utility;

namespace IndustryCodes
{
    public class SearchBySelectionController : Controller
    {
        private const int defaultSelectionValue = 0;
        private const string defaultSelectionText = "===  select one  ===";

        private IEnumerable<IndustrySector> industrySectors;
        private int selectedIndustrySector;
        private int previousSelectedIndustrySector;

        private IEnumerable<IndustrySubsector> industrySubsectors;
        private int selectedIndustrySubsector;
        private int previousSelectedIndustrySubsector;

        private IEnumerable<NAICSDescription> naicsDescriptions;
        private int selectedNAICSDescription;
        private int previousSelectedNAICSDescription;

        private IEnumerable<SICDescription> sicDescriptions;
        private int selectedSICDescription;

        public async Task<ActionResult> SearchBySelection(FormCollection collection)
        {
            var model = new SearchBySelectionViewModel();

            using (var context = new IndustryCodesContext())
            {
                if (FormCollectionHelper.IsFormButtonSelected("ClearButton", "Clear", collection))
                {
                    industrySectors = await GetIndustrySectors(context);

                    model.IndustrySectors = new SelectList(industrySectors, "Code", "Description");
                    model.SelectedIndustrySector = 0;

                    model.IndustrySubsectors = new SelectList(new Collection<IndustrySubsector>(), "Code", "Description");
                    model.SelectedIndustrySubsector = 0;

                    model.NAICSDescriptions = new SelectList(new Collection<NAICSDescription>(), "Code", "Description");
                    model.SelectedNAICSDescription = 0;

                    sicDescriptions = await GetSICDescriptions(context, null);

                    model.SICDescriptions = new SelectList(new Collection<SICDescription>(), "Code", "Description");
                    model.SelectedSICDescription = 0;

                    SetSelectionSessionVariables();

                    return View(model);
                }

                GetSelectionSessionVariables();

                industrySectors = await GetIndustrySectors(context);
                selectedIndustrySector = FormCollectionHelper.GetFormNumberValue("SelectedIndustrySector", collection);

                model.IndustrySectors = new SelectList(industrySectors, "Code", "Description");
                model.SelectedIndustrySector = selectedIndustrySector;

                if (selectedIndustrySector != previousSelectedIndustrySector)
                {
                    industrySubsectors = await GetIndustrySubsectors(context);

                    model.IndustrySubsectors = new SelectList(industrySubsectors, "Code", "Description");
                    model.SelectedIndustrySubsector = selectedIndustrySubsector;

                    naicsDescriptions = await GetNAICSDescriptions(context);

                    model.NAICSDescriptions = new SelectList(naicsDescriptions, "Code", "Description");
                    model.SelectedNAICSDescription = selectedNAICSDescription;

                    sicDescriptions = await GetSICDescriptions(context, null);

                    model.SICDescriptions = new SelectList(sicDescriptions, "Code", "Description");
                    model.SelectedSICDescription = selectedSICDescription;

                    SetSelectionSessionVariables();

                    return View(model);
                }

                industrySubsectors = await GetIndustrySubsectors(context);
                selectedIndustrySubsector = FormCollectionHelper.GetFormNumberValue("SelectedIndustrySubsector", collection);

                model.IndustrySubsectors = new SelectList(industrySubsectors, "Code", "Description");
                model.SelectedIndustrySubsector = selectedIndustrySubsector;

                if (selectedIndustrySubsector != previousSelectedIndustrySubsector)
                {
                    naicsDescriptions = await GetNAICSDescriptions(context);

                    model.NAICSDescriptions = new SelectList(naicsDescriptions, "Code", "Description");
                    model.SelectedNAICSDescription = selectedNAICSDescription;

                    sicDescriptions = await GetSICDescriptions(context, null);

                    model.SICDescriptions = new SelectList(sicDescriptions, "Code", "Description");
                    model.SelectedSICDescription = selectedSICDescription;

                    SetSelectionSessionVariables();

                    return View(model);
                }

                naicsDescriptions = await GetNAICSDescriptions(context);
                selectedNAICSDescription = FormCollectionHelper.GetFormNumberValue("SelectedNAICSDescription", collection);

                model.NAICSDescriptions = new SelectList(naicsDescriptions, "Code", "Description");
                model.SelectedNAICSDescription = selectedNAICSDescription;

                if (selectedNAICSDescription != previousSelectedNAICSDescription)
                {
                    sicDescriptions = await GetSICDescriptions(context, null);

                    model.SICDescriptions = new SelectList(sicDescriptions, "Code", "Description");
                    model.SelectedSICDescription = selectedSICDescription;

                    SetSelectionSessionVariables();

                    return View(model);
                }

                sicDescriptions = await GetSICDescriptions(context, null);
                selectedSICDescription = FormCollectionHelper.GetFormNumberValue("SelectedSICDescription", collection);

                model.SICDescriptions = new SelectList(sicDescriptions, "Code", "Description");
                model.SelectedSICDescription = selectedSICDescription;

                if (selectedNAICSDescription > 0 || selectedSICDescription > 0)
                {
                    model.NAICSCode = string.Format(CultureInfo.CurrentCulture, "{000000}", selectedNAICSDescription);
                    model.NAICSDescription = await GetNAICSDescription(context, null);
                    model.SICCode = string.Format(CultureInfo.CurrentCulture, "{0000}", selectedSICDescription);
                    model.SICDescription = await GetSICDescription(context, model.NAICSCode);
                }

                SetSelectionSessionVariables();
            }

            return View(model);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                return;
            }

            var model = new ErrorViewModel();

            if (filterContext.Exception != null)
            {
                var ex = filterContext.Exception;

                switch (ex)
                {
                    case EntityCommandExecutionException _:
                    case EntityException _:
                        {
                            model.ExceptionType = "Database entity exception";
                            model.ExceptionMessage = ex.Message;

                            if (ex.InnerException != null)
                            {
                                model.InnerExceptionMessage = ex.InnerException.Message;
                            }

                            var message = string.Format(CultureInfo.InvariantCulture, "({0}) {1}", model.ExceptionType, model.ExceptionMessage);
                            break;
                        }
                    case SqlException _:
                        {
                            model.ExceptionType = "SQL exception";
                            model.ExceptionMessage = ex.Message;

                            if (!string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("not currently available"))
                            {
                                model.ExceptionMessage = "Database is unavailable: please retry again later.";
                            }

                            if (ex.InnerException != null)
                            {
                                model.InnerExceptionMessage = ex.InnerException.Message;
                            }

                            var message = string.Format(CultureInfo.InvariantCulture, "({0}) {1}", model.ExceptionType, model.ExceptionMessage);
                            break;
                        }
                    default:
                        {
                            model.ExceptionType = "Unexpected exception";
                            model.ExceptionMessage = ex.Message;

                            if (ex.InnerException != null)
                            {
                                model.InnerExceptionMessage = ex.InnerException.Message;
                            }

                            var message = string.Format(CultureInfo.InvariantCulture, "({0}) {1}", model.ExceptionType, model.ExceptionMessage);
                            break;
                        }
                }
            }

            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult
            {
                ViewData = { Model = model },
                ViewName = "~/Views/Shared/Error.cshtml"
            };
        }

        private void GetSelectionSessionVariables()
        {
            if (Session["SelectedIndustrySector"] != null)
            {
                previousSelectedIndustrySector = (int)Session["SelectedIndustrySector"];
            }
            if (Session["SelectedIndustrySubsector"] != null)
            {
                previousSelectedIndustrySubsector = (int)Session["SelectedIndustrySubsector"];
            }
            if (Session["SelectedNAICSDescription"] != null)
            {
                previousSelectedNAICSDescription = (int)Session["SelectedNAICSDescription"];
            }
        }

        private void SetSelectionSessionVariables()
        {
            Session["SelectedIndustrySector"] = selectedIndustrySector;
            Session["SelectedIndustrySubsector"] = selectedIndustrySubsector;
            Session["SelectedNAICSDescription"] = selectedNAICSDescription;
        }

        private async Task<string> GetIndustrySector(IndustryCodesContext context)
        {
            var results = await context.ClassificationCodes
                                       .Select(c => c.INDUSTRY_SECTOR)
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

        private async Task<IEnumerable<IndustrySector>> GetIndustrySectors(IndustryCodesContext context)
        {
            var list = new Collection<IndustrySector>
            {
                new IndustrySector { Code = defaultSelectionValue, Description = defaultSelectionText }
            };

            var results = await context.ClassificationCodes
                                       .Select(c => c.INDUSTRY_SECTOR)
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

        private async Task<string> GetIndustrySubsector(IndustryCodesContext context)
        {
            var industrySector = await GetIndustrySector(context);

            var results = await context.ClassificationCodes
                                       .Where(c => c.INDUSTRY_SECTOR == industrySector)
                                       .Select(c => c.INDUSTRY_SUBSECTOR)
                                       .Distinct()
                                       .OrderBy(description => description)
                                       .ToListAsync();

            var index = 0;
            return results.FirstOrDefault(description => selectedIndustrySubsector == ++index);
        }

        private async Task<IEnumerable<IndustrySubsector>> GetIndustrySubsectors(IndustryCodesContext context)
        {
            var list = new Collection<IndustrySubsector>();

            if (selectedIndustrySector < 1)
            {
                return list;
            }

            list.Add(new IndustrySubsector { Code = defaultSelectionValue, Description = defaultSelectionText });

            var industrySector = await GetIndustrySector(context);

            var results = await context.ClassificationCodes
                                       .Where(c => c.INDUSTRY_SECTOR == industrySector)
                                       .Select(c => c.INDUSTRY_SUBSECTOR)
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

        private async Task<string> GetNAICSDescription(IndustryCodesContext context, string naicsCode)
        {
            if (string.IsNullOrEmpty(naicsCode) && selectedNAICSDescription <= 0)
            {
                return null;
            }

            var naicsCodeOrIndex = !string.IsNullOrEmpty(naicsCode) ? Assistant.GetNumberValue(naicsCode) : selectedNAICSDescription;

            var results = context.ClassificationCodes
                                 .Where(c => c.NORTH_AMERICAN_CODE == naicsCodeOrIndex)
                                 .Select(c => c.NORTH_AMERICAN_DESCRIPTION).Distinct();

            return await results.CountAsync() == 1 ? results.Single() : null;
        }

        private async Task<IEnumerable<NAICSDescription>> GetNAICSDescriptions(IndustryCodesContext context)
        {
            var list = new Collection<NAICSDescription>();

            if (selectedIndustrySubsector < 1)
            {
                return list;
            }

            list.Add(new NAICSDescription { Code = defaultSelectionValue, Description = defaultSelectionText });

            var industrySubsector = await GetIndustrySubsector(context);

            var results = await context.ClassificationCodes
                                       .Where(c => c.INDUSTRY_SUBSECTOR == industrySubsector)
                                       .Select(c => new { code = c.NORTH_AMERICAN_CODE, description = c.NORTH_AMERICAN_DESCRIPTION })
                                       .Distinct()
                                       .ToListAsync();

            foreach (var result in results)
            {
                if (result.code == null || result.description == null)
                {
                    continue;
                }

                var index = Convert.ToInt32(result.code, CultureInfo.CurrentCulture);
                var name = result.code + " - " + result.description;

                list.Add(new NAICSDescription { Code = index, Description = name });
            }
            return list;
        }

        private async Task<string> GetSICDescription(IndustryCodesContext context, string naicsCodeParameter)
        {
            if (selectedSICDescription < 1)
            {
                return null;
            }

            var naicsCode = Assistant.GetNumberValue(naicsCodeParameter);

            var sicCode = selectedSICDescription;

            var results = context.ClassificationCodes
                                 .Where(c => c.NORTH_AMERICAN_CODE == naicsCode && c.STANDARD_CODE == sicCode)
                                 .Select(c => new { description = c.STANDARD_DESCRIPTION });

            return await results.CountAsync() == 1 ? results.Single().description : null;
        }

        private async Task<IEnumerable<SICDescription>> GetSICDescriptions(IndustryCodesContext context, string naicsCode)
        {
            var list = new Collection<SICDescription>();

            if (string.IsNullOrEmpty(naicsCode) && selectedNAICSDescription <= 0)
            {
                return list;
            }

            var naicsCodeOrIndex = !string.IsNullOrEmpty(naicsCode) ? Assistant.GetNumberValue(naicsCode) : selectedNAICSDescription;

            list.Add(new SICDescription { Code = defaultSelectionValue, Description = defaultSelectionText });

            var results = await context.ClassificationCodes.Where(c => c.NORTH_AMERICAN_CODE == naicsCodeOrIndex)
                                       .Select(c => new { code = c.STANDARD_CODE, description = c.STANDARD_DESCRIPTION })
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
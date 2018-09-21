//
//  SearchByCodeController.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-18
//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using System.Web.Mvc;
using IndustryCodes.Database.Entities.Models;
using IndustryCodes.ViewModels;
using IndustryCodes.Utility;

namespace IndustryCodes
{
    public class SearchByCodeController : Controller
    {
        private const int defaultSelectionValue = 0;
        private const string defaultSelectionText = "===  select one  ===";

        private IEnumerable<SICDescription> sicDescriptions;
        private int selectedSICDescription;

        public async Task<ActionResult> SearchByCode(FormCollection collection)
        {
            var model = new SearchByCodeViewModel();

            if (FormCollectionHelper.IsFormButtonSelected("ClearButton", "Clear", collection))
            {
                return View(model);
            }

            model.NAICSCode = FormCollectionHelper.GetFormStringValue("NAICSCode", collection);
            model.NAICSDescription = FormCollectionHelper.GetFormStringValue("NAICSDescription", collection);

            using (var context = new IndustryCodesContext())
            {
                if (FormCollectionHelper.IsFormButtonSelected("SearchButton", "Search", collection))
                {
                    model.NAICSDescription = await GetNAICSDescription(context, model.NAICSCode);
                    if (model.NAICSDescription == null)
                    {
                        ModelState.AddModelError("ErrorMessage", string.Format(CultureInfo.CurrentCulture, "Unable to locate description for given NAICS code: {0}", model.NAICSCode));

                        model.NAICSCode = null;

                        return View(model);
                    }
                }

                sicDescriptions = await GetSICDescriptions(context, model.NAICSCode);
                selectedSICDescription = FormCollectionHelper.GetFormNumberValue("SelectedSICDescription", collection);

                if (sicDescriptions != null)
                {
                    model.SICDescriptions = new SelectList(sicDescriptions, "Code", "Description");
                }

                if (selectedSICDescription > 0)
                {
                    model.SelectedSICDescription = selectedSICDescription;
                }

                if (selectedSICDescription > 0)
                {
                    model.SICCode = string.Format(CultureInfo.CurrentCulture, "{0000}", selectedSICDescription);
                }

                if (selectedSICDescription > 0)
                {
                    model.SICDescription = GetSICDescription();
                }
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

        private static async Task<string> GetNAICSDescription(IndustryCodesContext context, string naicsCodeParameter)
        {
            if (string.IsNullOrEmpty(naicsCodeParameter))
            {
                return null;
            }

            var naicsCode = Assistant.GetNumberValue(naicsCodeParameter);

            var results = await context.ClassificationCodes
                                       .Where(c => c.NORTH_AMERICAN_CODE == naicsCode)
                                       .Select(c => c.NORTH_AMERICAN_DESCRIPTION)
                                       .Distinct()
                                       .ToListAsync();

            return results.Count() == 1 ? results.Single() : null;
        }

        private string GetSICDescription()
        {
            return sicDescriptions?.Where(s => s.Code == selectedSICDescription).Select(s => s.Description).FirstOrDefault();
        }

        private static async Task<IEnumerable<SICDescription>> GetSICDescriptions(IndustryCodesContext context, string naicsCodeParameter)
        {
            var list = new Collection<SICDescription>();

            if (string.IsNullOrEmpty(naicsCodeParameter))
            {
                return list;
            }

            var naicsCode = Assistant.GetNumberValue(naicsCodeParameter);

            list.Add(new SICDescription { Code = defaultSelectionValue, Description = defaultSelectionText });

            var results = await context.ClassificationCodes
                                       .Where(c => c.NORTH_AMERICAN_CODE == naicsCode)
                                       .Select(c => new { code = c.STANDARD_CODE, description = c.STANDARD_DESCRIPTION })
                                       .ToListAsync();

            foreach (var element in results)
            {
                if (element.code == null || element.description == null)
                {
                    continue;
                }

                var index = Convert.ToInt32(element.code, CultureInfo.CurrentCulture);
                var name = element.code + " - " + element.description;

                list.Add(new SICDescription { Code = index, Description = name });
            }
            return list;
        }
    }
}
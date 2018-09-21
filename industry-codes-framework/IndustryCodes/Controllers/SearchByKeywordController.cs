//
//  SearchByKeywordController.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-18
//
using System;
using System.Collections.Generic;
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
    public class SearchByKeywordController : Controller
    {
        public async Task<ActionResult> SearchByKeyword(FormCollection collection, bool? useCache)
        {
            var model = new SearchByKeywordViewModel();

            var clear = FormCollectionHelper.IsFormButtonSelected("ClearButton", "Clear", collection);

            if (useCache != null && useCache == true)
            {
                GetSelectByKeywordSessionVariables(model);

                return View(model);
            }

            RemoveSelectByKeywordSessionVariables();

            if (clear)
            {
                SetSearchedValues(model, collection, true);

                return View(model);
            }

            using (var context = new IndustryCodesContext())
            {
                SetSearchedValues(model, collection, false);

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    model.SearchResults = await FindByKeyword(context, model.Keyword, model.SearchNAICS);
                }

                SetSelectByKeywordSessionVariables(model);
            }

            return View(model);
        }

        public async Task<ActionResult> SearchDetail(string id)
        {
            var model = new SearchDetailViewModel();

            using (var context = new IndustryCodesContext())
            {
                var classificationCode = await FindByIdentifier(context, id);
                if (classificationCode != null)
                {
                    model.NAICSCode = string.Format(CultureInfo.CurrentCulture, "{000000}", Convert.ToString(classificationCode.NORTH_AMERICAN_CODE, CultureInfo.CurrentCulture));
                    model.NAICSDescription = classificationCode.NORTH_AMERICAN_DESCRIPTION;
                    model.SICCode = string.Format(CultureInfo.CurrentCulture, "{0000}", classificationCode.STANDARD_CODE);
                    model.SICDescription = classificationCode.STANDARD_DESCRIPTION;
                }
                else
                {
                    ModelState.AddModelError("ErrorMessage", string.Format(CultureInfo.CurrentCulture, "Unable to locate record for given identifier: {0}", id));
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

        private static void SetSearchedValues(SearchByKeywordViewModel model, FormCollection collection, bool optionOnly)
        {
            if (!optionOnly)
            {
                model.Keyword = FormCollectionHelper.GetFormStringValue("Keyword", collection);
            }

            var keywordOption = FormCollectionHelper.GetFormRadioButtonValue("KeywordOption", collection);

            if (!string.IsNullOrEmpty(keywordOption))
            {
                if (keywordOption.Equals("NAICS"))
                {
                    model.SearchNAICS = true;
                }
                if (keywordOption.Equals("SIC"))
                {
                    model.SearchSIC = true;
                }
            }

            if (model.SearchNAICS == false && model.SearchSIC == false)
            {
                model.SearchNAICS = true;
            }
        }

        private void GetSelectByKeywordSessionVariables(SearchByKeywordViewModel model)
        {
            if (Session["Keyword"] != null)
            {
                model.Keyword = (string)Session["Keyword"];
            }
            if (Session["KeywordSearchResults"] != null)
            {
                model.SearchResults = (List<ClassificationCode>)Session["KeywordSearchResults"];
            }
            if (Session["KeywordSearchByNAICS"] != null)
            {
                model.SearchNAICS = (bool)Session["KeywordSearchByNAICS"];
            }
            if (Session["KeywordSearchBySIC"] != null)
            {
                model.SearchSIC = (bool)Session["KeywordSearchBySIC"];
            }
        }

        private void SetSelectByKeywordSessionVariables(SearchByKeywordViewModel model)
        {
            Session["Keyword"] = model.Keyword;
            Session["KeywordSearchResults"] = model.SearchResults;
            Session["KeywordSearchByNAICS"] = model.SearchNAICS;
            Session["KeywordSearchBySIC"] = model.SearchSIC;
        }

        private void RemoveSelectByKeywordSessionVariables()
        {
            Session.Remove("Keywords");
            Session.Remove("KeywordSearchResults");
            Session.Remove("KeywordSearchByNAICS");
            Session.Remove("KeywordSearchBySIC");
        }

        private static async Task<List<ClassificationCode>> FindByKeyword(IndustryCodesContext context, string keyword, bool searchNAICS)
        {
            if (searchNAICS)
            {
                var results = context.ClassificationCodes
                                     .Where(c => c.NORTH_AMERICAN_DESCRIPTION.Contains(keyword))
                                     .Select(c => c);

                return await results.ToListAsync();
            }
            else
            {
                var results = context.ClassificationCodes
                                     .Where(c => c.STANDARD_DESCRIPTION.Contains(keyword))
                                     .Select(c => c);

                return await results.ToListAsync();
            }
        }

        private static async Task<ClassificationCode> FindByIdentifier(IndustryCodesContext context, string identifier)
        {
            var id = Assistant.GetNumberValue(identifier);

            return id < 1 ? null : await context.ClassificationCodes.FindAsync(id);
        }
    }
}
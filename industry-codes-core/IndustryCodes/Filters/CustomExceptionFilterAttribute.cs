//
//  CustomExceptionFilterAttribute.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
//
using System;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using IndustryCodes.ViewModels;

namespace IndustryCodes
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilterAttribute> logger; 
        private readonly IModelMetadataProvider modelMetadataProvider;

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> applicationLogger, IModelMetadataProvider metadataProvider)
        {
            logger = applicationLogger ?? throw new ArgumentNullException(nameof(applicationLogger));   
            modelMetadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
        }

        public override void OnException(ExceptionContext filterContext)
        {       
            if (filterContext == null)
            {
                return;
            }

            var errorViewModel = new ErrorViewModel();

            if (filterContext.Exception != null)
            {
                var exception = filterContext.Exception;

                switch (exception)
                {
                    case SqlException _:
                        {
                            errorViewModel.ExceptionName = "SQL exception";
                            errorViewModel.ExceptionMessage = CustomDatabaseErrorMessage(exception.Message);

                            if (exception.InnerException != null)
                            {
                                errorViewModel.ExceptionSourceMessage = exception.InnerException.Message;
                            }

                            var message = string.Format(CultureInfo.InvariantCulture, "({0}) {1}", errorViewModel.ExceptionName, errorViewModel.ExceptionMessage);
                            logger.LogError(message);
                            break;
                        }
                    case InvalidOperationException _:
                        {
                            errorViewModel.ExceptionName = "Invalid operation exception";
                            errorViewModel.ExceptionMessage = exception.Message;

                            if (exception.InnerException != null)
                            {
                                errorViewModel.ExceptionSourceMessage = exception.InnerException.Message;
                            }

                            var message = string.Format(CultureInfo.InvariantCulture, "({0}) {1}", errorViewModel.ExceptionName, errorViewModel.ExceptionMessage);
                            logger.LogError(message);
                            break;
                        }
                    default:
                        {
                            errorViewModel.ExceptionName = "Unexpected exception";
                            errorViewModel.ExceptionMessage = exception.Message;

                            if (exception.InnerException != null)
                            {
                                errorViewModel.ExceptionSourceMessage = exception.InnerException.Message;
                            }

                            var message = string.Format(CultureInfo.InvariantCulture, "({0}) {1}", errorViewModel.ExceptionName, errorViewModel.ExceptionMessage);
                            logger.LogError(message);
                            break;
                        }
                }
            }

            errorViewModel.RequestId = filterContext.HttpContext.TraceIdentifier;

            var viewResult = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(modelMetadataProvider, filterContext.ModelState)
            };

            viewResult.ViewData.Add("ViewModel", errorViewModel);

            filterContext.ExceptionHandled = true;
            filterContext.Result = viewResult;
        }

        private string CustomDatabaseErrorMessage(string message)
        {
            var errorMessage = message;

            if (!string.IsNullOrEmpty(message) && message.Contains("server was not found or was not accessible"))
            {
                errorMessage = "Database is unavailable: please retry again later.";
            }

            return errorMessage;
        }
    }
}
//
//  ErrorViewModel.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-18
//
using System.ComponentModel.DataAnnotations;

namespace IndustryCodes.ViewModels
{
    public class ErrorViewModel
    {
        [Display(Name = "Exception Type")]
        public string ExceptionType { get; set; }

        [Display(Name = "Exception Message")]
        public string ExceptionMessage { get; set; }

        [Display(Name = "Inner Exception Message")]
        public string InnerExceptionMessage { get; set; }
    }
}
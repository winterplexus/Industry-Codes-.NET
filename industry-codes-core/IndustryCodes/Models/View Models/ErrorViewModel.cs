//
//  ErrorViewModel.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2019
//
using System.ComponentModel.DataAnnotations;

namespace IndustryCodes.ViewModels
{
    public class ErrorViewModel
    {
        [Display(Name = "Request ID")]
        public string RequestId { get; set; }

        [Display(Name = "Exception Name")]
        public string ExceptionName { get; set; }

        [Display(Name = "Exception Message")]
        public string ExceptionMessage { get; set; }

        [Display(Name = "Exception Source Message")]
        public string ExceptionSourceMessage { get; set; }
    }
}
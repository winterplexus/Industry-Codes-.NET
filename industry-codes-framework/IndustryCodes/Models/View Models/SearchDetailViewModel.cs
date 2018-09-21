//
//  SearchDetailViewModel.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-18
//
using System.ComponentModel.DataAnnotations;

namespace IndustryCodes.ViewModels
{
    public class SearchDetailViewModel
    {
        [Display(Name = "NAICS Code")]
        public string NAICSCode { get; set; }

        [Display(Name = "NAICS Description")]
        public string NAICSDescription { get; set; }

        [Display(Name = "SIC Code")]
        public string SICCode { get; set; }

        [Display(Name = "SIC Description")]
        public string SICDescription { get; set; }
    }
}
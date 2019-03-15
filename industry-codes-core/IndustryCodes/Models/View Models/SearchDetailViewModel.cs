//
//  SearchDetailViewModel.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2019
//
using System.ComponentModel.DataAnnotations;

namespace IndustryCodes.ViewModels
{
    public class SearchDetailViewModel
    {
        [StringLength(255)]
        public string Keyword { get; set; }

        [StringLength(5)]
        public string KeywordOption { get; set; }

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
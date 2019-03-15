//
//  SearchByKeywordViewModel.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2019
//
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IndustryCodes.Models;

namespace IndustryCodes.ViewModels
{
    public class SearchByKeywordViewModel
    {
        [Display(Name = "Keyword")]
        [StringLength(255)]
        [Required(ErrorMessage = "Please enter either a NAICS or SIC description keyword")]
        public string Keyword { get; set; }

        [StringLength(5)]
        public string KeywordOption { get; set; }

        [Display(Name = "Search NAICS Descriptions")]
        public bool SearchNAICS { get; set; }

        [Display(Name = "Search SIC Descriptions")]
        public bool SearchSIC { get; set; }

        public List<ClassificationCodes> SearchResults { get; set; }
    }
}
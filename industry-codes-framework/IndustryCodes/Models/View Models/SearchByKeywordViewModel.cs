//
//  SearchByKeywordViewModel.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-18
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IndustryCodes.Database.Entities.Models;

namespace IndustryCodes.ViewModels
{
    public class SearchByKeywordViewModel
    {
        [Display(Name = "Keyword")]
        [StringLength(255)]
        public string Keyword { get; set; }

        [Display(Name = "Search NAICS Descriptions")]
        public bool SearchNAICS { get; set; }

        [Display(Name = "Search SIC Descriptions")]
        public bool SearchSIC { get; set; }

        [CLSCompliant(false)]
        public List<ClassificationCode> SearchResults { get; set; }
    }
}
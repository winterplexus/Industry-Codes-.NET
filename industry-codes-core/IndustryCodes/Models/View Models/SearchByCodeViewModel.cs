//
//  SearchByCodeViewModel.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
//
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IndustryCodes.ViewModels
{
    public class SearchByCodeViewModel
    {
        [Display(Name = "NAICS Code")]
        [StringLength(6)]
        [Required(ErrorMessage = "Please enter a NAICS code")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "NAICS code must be a number")]
        public string NAICSCode { get; set; }

        [Display(Name = "NAICS Description")]
        public string NAICSDescription { get; set; }

        [Display(Name = "SIC Code")]
        public string SICCode { get; set; }

        [Display(Name = "SIC Description")]
        public string SICDescription { get; set; }

        [Display(Name = "SIC Descriptions")]
        public SelectList SICDescriptions { get; set; }

        [Display(Name = "Selected SIC Description")]
        public int SelectedSICDescription { get; set; }
    }
}
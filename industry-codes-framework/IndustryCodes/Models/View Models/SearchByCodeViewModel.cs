//
//  SearchByCodeViewModel.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-18
//
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IndustryCodes.ViewModels
{
    public class SearchByCodeViewModel
    {
        [Display(Name = "SIC Descriptions")]
        public SelectList SICDescriptions { get; set; }

        [Display(Name = "Selected SIC Description")]
        public int SelectedSICDescription { get; set; }

        [Display(Name = "NAICS Code")]
        [StringLength(6)]
        [Required(ErrorMessage = "Please enter a NAICS code")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "NAICS code is a number: non-numeric characters are not allowed")]
        public string NAICSCode { get; set; }

        [Display(Name = "NAICS Description")]
        public string NAICSDescription { get; set; }

        [Display(Name = "SIC Code")]
        [StringLength(4)]
        [Required(ErrorMessage = "Please enter a SIC code")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "SIC code is a number: non-numeric characters are not allowed")]
        public string SICCode { get; set; }

        [Display(Name = "SIC Description")]
        public string SICDescription { get; set; }
    }
}
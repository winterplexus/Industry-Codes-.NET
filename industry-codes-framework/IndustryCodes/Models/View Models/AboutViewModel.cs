//
//  AboutViewModel.cs
//
//  Copyright (c) Wiregrass Code Technology 2014-18
//
using System.ComponentModel.DataAnnotations;

namespace IndustryCodes.ViewModels
{
    public class AboutViewModel
    {
        [Display(Name = "Application")]
        public string Application { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Version")]
        public string Version { get; set; }

        [Display(Name = "Copyright")]
        public string Copyright { get; set; }
    }
}
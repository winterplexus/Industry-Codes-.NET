//
//  About.cshtml.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2019
//   
using IndustryCodes.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace IndustryCodes.Pages
{
    public class AboutModel : PageModel
    {
        public IActionResult OnGet()
        {
            ViewModel = new AboutViewModel();

            SetAboutValues();

            return Page();
        }

        [BindProperty]
        public AboutViewModel ViewModel { get; set; }

        private void SetAboutValues()
        {
            ViewModel.Version = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

            var titleAttributes = Assembly.GetEntryAssembly()?.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (titleAttributes != null && titleAttributes.Length > 0)
            {
                ViewModel.Application = ((AssemblyTitleAttribute)titleAttributes[0]).Title;
            }
            var descriptionAttributes = Assembly.GetEntryAssembly()?.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (descriptionAttributes != null && descriptionAttributes.Length > 0)
            {
                ViewModel.Description = ((AssemblyDescriptionAttribute)descriptionAttributes[0]).Description;
            }
            var copyrightAttributes = Assembly.GetEntryAssembly()?.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (copyrightAttributes != null && copyrightAttributes.Length > 0)
            {
                ViewModel.Copyright = ((AssemblyCopyrightAttribute)copyrightAttributes[0]).Copyright;
            }
        }
    }
}
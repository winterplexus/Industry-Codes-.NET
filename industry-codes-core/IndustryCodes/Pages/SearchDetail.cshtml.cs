//
//  SearchDetail.cshtml.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
// 
using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IndustryCodes.Models;
using IndustryCodes.Utility;
using IndustryCodes.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace IndustryCodes.Pages
{
    public class SearchDetailModel : PageModel
    {
        private readonly IndustryCodesContext context;

        public SearchDetailModel(IndustryCodesContext industryCodesContext)
        {
            context = industryCodesContext ?? throw new ArgumentNullException(nameof(industryCodesContext));
        }

        public async Task<IActionResult> OnGetAsync(int id, string keyword, string option)
        {
            ViewModel = new SearchDetailViewModel();

            var classificationCode = await FindByIdentifier(id);
            if (classificationCode != null)
            {
                ViewModel.Keyword = keyword;
                ViewModel.KeywordOption = option;
                ViewModel.NAICSCode = string.Format(CultureInfo.CurrentCulture, "{000000}", classificationCode.NorthAmericanCode);
                ViewModel.NAICSDescription = classificationCode.NorthAmericanDescription;
                ViewModel.SICCode = string.Format(CultureInfo.CurrentCulture, "{0000}", classificationCode.StandardCode);
                ViewModel.SICDescription = classificationCode.StandardDescription;
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", string.Format(CultureInfo.CurrentCulture, "Unable to locate record for given identifier: {0}", id));
            }

            return Page();
        }

        public IActionResult OnPost(IFormCollection collection)
        {
            if (FormCollectionAssistant.IsFormButtonSelected("BackButton", "Back", collection))
            {
                return RedirectToPage("/SearchByKeyword", "Keyword", new { keyword = ViewModel.Keyword, option = ViewModel.KeywordOption });
            }

            return Page();
        }

        [BindProperty]
        public SearchDetailViewModel ViewModel { get; set; }

        private async Task<ClassificationCodes> FindByIdentifier(int id)
        {
            return id < 1 ? null : await context.ClassificationCodes.FirstAsync(c => c.Id == id);
        }
    }
}
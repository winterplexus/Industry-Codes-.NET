//
//  SearchByKeyword.cshtml.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
//       
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IndustryCodes.Models;
using IndustryCodes.ViewModels;
using IndustryCodes.Utility;
using Microsoft.EntityFrameworkCore;

namespace IndustryCodes.Pages
{
    public class SearchByKeywordModel : PageModel
    {
        private readonly IndustryCodesContext context;

        public SearchByKeywordModel(IndustryCodesContext industryCodesContext)
        {
            context = industryCodesContext ?? throw new ArgumentNullException(nameof(industryCodesContext));
        }

        public IActionResult OnGet()
        {
            ViewModel = new SearchByKeywordViewModel { SearchResults = new List<ClassificationCodes>(), SearchNAICS = true };

            return Page();
        }

        public async Task<IActionResult> OnGetKeywordAsync(string keyword, string option)
        {
            ViewModel = new SearchByKeywordViewModel { SearchResults = new List<ClassificationCodes>() };

            if (!string.IsNullOrEmpty(keyword))
            {
                ViewModel.Keyword = keyword;
                ViewModel.KeywordOption = option;

                if (!string.IsNullOrEmpty(option))
                {
                    if (option.Equals("NAICS"))
                    {
                        ViewModel.SearchNAICS = true;
                    }
                    if (option.Equals("SIC"))
                    {
                        ViewModel.SearchSIC = true;
                    }
                }

                ViewModel.SearchResults = await FindByKeyword(ViewModel.Keyword, ViewModel.SearchNAICS);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection collection)
        {
            if (FormCollectionAssistant.IsFormButtonSelected("ClearButton", "Clear", collection))
            {
                return RedirectToPage("/SearchByKeyword");
            }

            ViewModel.Keyword = FormCollectionAssistant.GetFormStringValue("ViewModel.Keyword", collection);

            var keywordOption = FormCollectionAssistant.GetFormRadioButtonValue("KeywordOption", collection);
            if (!string.IsNullOrEmpty(keywordOption))
            {
                if (keywordOption.Equals("NAICS"))
                {
                    ViewModel.SearchNAICS = true;
                }
                if (keywordOption.Equals("SIC"))
                {
                    ViewModel.SearchSIC = true;
                }
            }
            if (ViewModel.SearchNAICS == false && ViewModel.SearchSIC == false)
            {
                ViewModel.SearchNAICS = true;
            }

            ViewModel.KeywordOption = ViewModel.SearchNAICS ? "NAICS" : "SIC";

            if (!string.IsNullOrEmpty(ViewModel.Keyword))
            {
                ViewModel.SearchResults = await FindByKeyword(ViewModel.Keyword, ViewModel.SearchNAICS);
            }

            return Page();
        }

        [BindProperty]
        public SearchByKeywordViewModel ViewModel { get; set; }

        private async Task<List<ClassificationCodes>> FindByKeyword(string keyword, bool searchNAICS)
        {
            if (searchNAICS)
            {
                var results = context.ClassificationCodes
                                     .Where(c => c.NorthAmericanDescription
                                     .Contains(keyword))
                                     .Select(c => c);

                return await results.ToListAsync();
            }
            else
            {
                var results = context.ClassificationCodes
                                     .Where(c => c.StandardDescription
                                     .Contains(keyword))
                                     .Select(c => c);

                return await results.ToListAsync();
            }
        }
    }
}
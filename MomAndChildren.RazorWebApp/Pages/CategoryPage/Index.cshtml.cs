using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.CategoryPage
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryBusiness business;

        public IndexModel()
        {
            business ??= new CategoryBusiness();
        }

        public IEnumerable<Category> Category { get;set; } = default!;

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        public string? Name { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string searchTerm = "")
        {
            var result = await business.GetCategoriesAsync();

            //Paging
            CurrentPage = currentPage;
            int pageSize = 5;

            //Search
            if (result != null && result.Status > 0 && result.Data != null)
            {
                Category = result.Data as List<Category>;
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = await business.SearchByKeyword(searchTerm);
                Category = result.Data as List<Category>;

                return Page();
            }

            int totalCategory = Category.Count();
            TotalPages = (int)Math.Ceiling((double)totalCategory / pageSize);

            Category = Category.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return Page();
        }
    }
}

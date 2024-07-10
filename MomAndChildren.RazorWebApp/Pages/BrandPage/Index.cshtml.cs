using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.BrandPage
{
    public class IndexModel : PageModel
    {
        private readonly IBrandBusiness business;

        public IndexModel()
        {
            business ??= new BrandBusiness();
        }

        public IEnumerable<Brand> Brand { get; set; } = default!;

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        public string? Name { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string searchTerm = "")
        {
            var result = await business.GetBrandsAsync();

            //Paging
            CurrentPage = currentPage;
            int pageSize = 5;

            //Search
            if (result != null && result.Status > 0 && result.Data != null)
            {
                Brand = result.Data as List<Brand>;
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = await business.SearchByKeyword(searchTerm);
                Brand = result.Data as List<Brand>;

                return Page();
            }

            int totalBrand = Brand.Count();
            TotalPages = (int)Math.Ceiling((double)totalBrand / pageSize);

            Brand = Brand.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return Page();
        }
    }
}

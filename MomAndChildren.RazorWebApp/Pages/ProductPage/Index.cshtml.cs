using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.ProductPage
{
    public class IndexModel : PageModel
    {
        private readonly IProductBusiness business;

        public IndexModel()
        {
            business ??= new ProductBusiness();
        }

        public IEnumerable<Product> Product { get;set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public string? Name { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string searchTerm = "")
        {
            var result = await business.GetProductsWithNestedObjAsync();

            //Paging
            CurrentPage = currentPage;
            int pageSize = 5;

            //Search
            if (result != null && result.Status > 0 && result.Data != null)
            {
                Product = result.Data as List<Product>;
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = await business.SearchByKeyword(searchTerm);
                Product = result.Data as List<Product>;

                return Page();
            }

            int totalProduct = Product.Count();
            TotalPages = (int)Math.Ceiling((double)totalProduct / pageSize);

            Product = Product.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return Page();
        }
    }
}

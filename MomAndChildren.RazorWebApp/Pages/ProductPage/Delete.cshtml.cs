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
    public class DeleteModel : PageModel
    {
        private readonly IProductBusiness business;

        public DeleteModel()
        {
            business = new ProductBusiness();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await business.GetProductByIdWithNestedObjAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product.Data as Product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await business.DeleteProduct(id);

            return RedirectToPage("./Index");
        }
    }
}

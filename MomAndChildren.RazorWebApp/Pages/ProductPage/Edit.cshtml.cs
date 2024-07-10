using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.ProductPage
{
    public class EditModel : PageModel
    {
        private readonly IProductBusiness productBusiness;
        private readonly ICategoryBusiness categoryBusiness;
        private readonly IBrandBusiness brandBusiness;

        public EditModel()
        {
            productBusiness ??= new ProductBusiness();
            categoryBusiness ??= new CategoryBusiness();
            brandBusiness ??= new BrandBusiness();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public SelectList ProductStatusOptions { get; set; } = default!;

        public SelectList CategoryStatusOptions { get; set; } = default!;

        public SelectList BrandStatusOptions { get; set; } = default!;

        public IActionResult OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = productBusiness.GetProductByIdWithNestedObjAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            List<Category> categories = new List<Category>();
            categories = categoryBusiness.GetCategoriesAsync().Result.Data as List<Category>;

            List<Brand> brands = new List<Brand>();
            brands = brandBusiness.GetBrandsAsync().Result.Data as List<Brand>;

            ProductStatusOptions = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Active" },
                new SelectListItem { Value = "0", Text = "Inactive" },
            }, "Value", "Text");

            List<SelectListItem> listCategories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();
            CategoryStatusOptions = new SelectList(listCategories, "Value", "Text");

            List<SelectListItem> listBrands = brands.Select(b => new SelectListItem
            {
                Value = b.BrandId.ToString(),
                Text = b.BrandName
            }).ToList();
            BrandStatusOptions = new SelectList(listBrands, "Value", "Text");

            Product = product.Result.Data as Product;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            await productBusiness.UpdateProduct(Product);

            return RedirectToPage("./Index");
        }
    }
}

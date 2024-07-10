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
    public class DeleteModel : PageModel
    {
        private readonly IBrandBusiness business;

        public DeleteModel()
        {
            business = new BrandBusiness();
        }

        [BindProperty]
        public Brand Brand { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await business.GetBrandByIdAsync(id);

            if (brand == null)
            {
                return NotFound();
            }
            else
            {
                Brand = brand.Data as Brand;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var brand = await business.DeleteBrand(id);

            return RedirectToPage("./Index");
        }
    }
}

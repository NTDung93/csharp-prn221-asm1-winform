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

namespace MomAndChildren.RazorWebApp.Pages.BrandPage
{
    public class EditModel : PageModel
    {
        private readonly IBrandBusiness business;

        public EditModel()
        {
            business ??= new BrandBusiness();
        }

        [BindProperty]
        public Brand Brand { get; set; } = default!;

        public SelectList BrandStatusOptions { get; set; } = default!;

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

            BrandStatusOptions = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Active" },
                new SelectListItem { Value = "0", Text = "Inactive" }
            }, "Value", "Text");
            Brand = brand.Data as Brand;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await business.UpdateBrand(Brand);
            }
            catch (Exception ex)
            {
            }

            return RedirectToPage("./Index");
        }
    }
}

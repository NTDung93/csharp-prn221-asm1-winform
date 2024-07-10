using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.BrandPage
{
    public class CreateModel : PageModel
    {
        private readonly IBrandBusiness business;

        public CreateModel()
        {
            business ??= new BrandBusiness();
        }

        [BindProperty]
        public Brand Brand { get; set; } = default!;

        public SelectList BrandStatusOptions { get; set; } = default!;

        public IActionResult OnGet()
        {
            BrandStatusOptions = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Active" },
                new SelectListItem { Value = "0", Text = "Inactive" },
            }, "Value", "Text");

            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await business.CreateBrand(Brand);

            return RedirectToPage("./Index");
        }
    }
}

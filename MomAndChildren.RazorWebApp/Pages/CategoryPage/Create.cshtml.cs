using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.CategoryPage
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryBusiness business;

        public CreateModel()
        {
            business ??= new CategoryBusiness();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public SelectList CategoryStatusOptions { get; set; } = default!;

        public IActionResult OnGet()
        {
            CategoryStatusOptions = new SelectList(new List<SelectListItem>
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

            await business.CreateCategory(Category);

            return RedirectToPage("./Index");
        }
    }
}

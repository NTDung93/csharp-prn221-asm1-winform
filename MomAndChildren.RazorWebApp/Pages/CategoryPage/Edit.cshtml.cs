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

namespace MomAndChildren.RazorWebApp.Pages.CategoryPage
{
    public class EditModel : PageModel
    {
        private readonly ICategoryBusiness business;

        public EditModel()
        {
            business ??= new CategoryBusiness();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public SelectList CategoryStatusOptions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await business.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            CategoryStatusOptions = new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Active" },
            new SelectListItem { Value = "0", Text = "Inactive" }
        }, "Value", "Text");
            Category = category.Data as Category;
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
                await business.UpdateCategory(Category);
            }
            catch (Exception ex)
            {
            }

            return RedirectToPage("./Index");
        }
    }
}

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
    public class DeleteModel : PageModel
    {
        private readonly ICategoryBusiness business;

        public DeleteModel()
        {
            business = new CategoryBusiness();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

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
            else
            {
                Category = category.Data as Category;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await business.DeleteCategory(id);

            return RedirectToPage("./Index");
        }
    }
}

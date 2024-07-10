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
    public class DetailsModel : PageModel
    {
        private readonly ICategoryBusiness business;

        public DetailsModel()
        {
            business ??= new CategoryBusiness();
        }

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
    }
}

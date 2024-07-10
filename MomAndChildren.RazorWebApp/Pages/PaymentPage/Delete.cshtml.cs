using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.PaymentPage
{
    public class DeleteModel : PageModel
    {
        private readonly IPaymentBusiness paymentBusiness;

        public DeleteModel()
        {
            paymentBusiness ??= new PaymentBusiness();
        }

        [BindProperty]
        public Payment Payment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await paymentBusiness.GetPaymentByIdAsync((int)id);

            if (payment == null)
            {
                return NotFound();
            }
            else
            {
                Payment = (Payment)payment.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var paymentHistory = await paymentBusiness.RemovePayment((int)id);

            return RedirectToPage("./Index");
        }
    }
}

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
    public class DetailsModel : PageModel
    {
        private readonly IPaymentBusiness PaymentBusiness;

        public DetailsModel()
        {
            PaymentBusiness ??= new PaymentBusiness();
        }

        public Payment Payment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await PaymentBusiness.GetPaymentByIdAsync((int)id);
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
    }
}

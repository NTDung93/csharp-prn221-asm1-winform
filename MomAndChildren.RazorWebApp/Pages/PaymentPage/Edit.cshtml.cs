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

namespace MomAndChildren.RazorWebApp.Pages.PaymentPage
{
    public class EditModel : PageModel
    {
        private readonly IPaymentBusiness PaymentBusiness;
        private readonly IOrderBusiness orderBusiness;

        public EditModel()
        {
            orderBusiness ??= new OrderBusiness();
            PaymentBusiness ??= new PaymentBusiness();
        }

        [BindProperty]
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

            ViewData["OrderId"] = new SelectList((List<Order>)orderBusiness.GetOrdersAsync().Result.Data, "OrderId", "OrderId", "selectedValue");
            Payment = (Payment)payment.Data;

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
                await PaymentBusiness.UpdatePayment(Payment);
            }
            catch (Exception ex)
            {

            }

            return RedirectToPage("./Index");
        }

        //private bool PaymentExists(int id)
        //{
        //  return (_context.PaymentHistories?.Any(e => e.PaymentId == id)).GetValueOrDefault();
        //}
    }
}

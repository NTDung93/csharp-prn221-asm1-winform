using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.PaymentPage
{
    public class CreateModel : PageModel
    {
        private readonly IPaymentBusiness paymentBusiness;
        private readonly IOrderBusiness orderBusiness;

        public CreateModel()
        {
            orderBusiness ??= new OrderBusiness();
            paymentBusiness ??= new PaymentBusiness();
        }

        public IActionResult OnGetAsync()
        {
            ViewData["StatusOptions"] = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "isPaid" },
                new SelectListItem { Value = "0", Text = "onHold" }
            };
            ViewData["OrderId"] = new SelectList((List<Order>)orderBusiness.GetOrdersAsync().Result.Data, "OrderId", "OrderId");
            return Page();
        }

        [BindProperty]
        public Payment Payment { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["StatusOptions"] = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "isPaid" },
                    new SelectListItem { Value = "0", Text = "onHold" }
                };

                var orders = orderBusiness.GetOrdersAsync().Result.Data as List<Order>;
                ViewData["OrderId"] = new SelectList(orders, "OrderId", "OrderId");

                return Page();
            }
            await paymentBusiness.CreatePayment(Payment);

            return RedirectToPage("./Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.PaymentPage
{
    public class IndexModel : PageModel
    {
        private readonly IPaymentBusiness PaymentBusiness;

        public IndexModel()
        {
            PaymentBusiness ??= new PaymentBusiness();
        }

        public IList<Payment> Payment { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await PaymentBusiness.GetPaymentList();
            if (result != null && result.Status > 0 && result.Data != null)
            {
                Payment = (List<Payment>)result.Data;
            }
        }
    }
}

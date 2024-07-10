using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomAndChildren.Business;
using MomAndChildren.Common;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages
{
    public class PaymentModel : PageModel
    {

        private readonly IPaymentBusiness _paymentBusiness;
        private readonly IOrderBusiness _orderBusiness;

        public PaymentModel(IPaymentBusiness paymentBusiness, IOrderBusiness orderBusiness)
        {
            _paymentBusiness = paymentBusiness;
            _orderBusiness = orderBusiness;
        }


        [BindProperty]
        public Payment Payment { get; set; }
        public List<Payment> PaymentList { get; set; }
        public List<SelectListItem> OrderList { get; set; }
        public string? Message { get; set; }

        public void OnGet()
        {
            PaymentList = this.GetPaymentList();
            OrderList = this.GetOrders();
        }

        public void OnPost()
        {
            this.CreatePayment();
        }

        public void OnPostUpdate()
        {
            this.UpdatePayment();
        }

        public void OnPostDelete(int paymentId)
        {
            this.DeletePayment(paymentId);
        }

        private List<SelectListItem> GetOrders()
        {
            var orderResult = _orderBusiness.GetAllOrdersWherePaymentIsNotNullAsync();
            var orders = new List<SelectListItem>();

            if (orderResult.Result.Status == Const.SUCCESS_READ_CODE && orderResult.Result.Data is IEnumerable<Order> orderList)
            {
                foreach (var order in orderList)
                {
                    orders.Add(new SelectListItem
                    {
                        Value = order.OrderId.ToString(),
                        Text = $"Order #{order.OrderId} - {order.OrderDate.ToShortDateString()}"
                    });
                }
            }
            return orders;
        }

        private List<Payment> GetPaymentList()
        {
            var result = _paymentBusiness.GetPaymentList();

            if (result.Status > 0 && result.Result.Data != null)
            {
                var paymentHistories = (List<Payment>)result.Result.Data;
                return paymentHistories;
            }
            return new List<Payment>();
        }

        private void CreatePayment()
        {
            var result = _paymentBusiness.CreatePayment(Payment);

            if (result != null)
            {
                Message = result.Result.Message;
            }
            else
            {
                this.Message = "Error system";
            }
        }
        private void UpdatePayment()
        {
            var result = _paymentBusiness.UpdatePayment(Payment);

            if (result != null)
            {
                Message = result.Result.Message;
            }
            else
            {
                this.Message = "Error system";
            }
        }

        private void DeletePayment(int paymentId)
        {
            var result = _paymentBusiness.RemovePayment(paymentId);

            if (result != null)
            {
                Message = result.Result.Message;
            }
            else
            {
                this.Message = "Error system";
            }
        }
    }
}

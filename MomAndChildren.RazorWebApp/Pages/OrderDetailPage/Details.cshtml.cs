using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.OrderDetailPage
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderDetailBusiness _orderDetail;

        public DetailsModel(IOrderDetailBusiness orderDetail)
        {
            _orderDetail = orderDetail;
        }

        public OrderDetail OrderDetail { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var orderDetail = _orderDetail.GetOrderDetailByIdAsync(id);
            if (id == null || orderDetail == null)
            {
                return NotFound();
            }
            else 
            {
                OrderDetail = orderDetail.Result.Data as OrderDetail;
            }
            return Page();
        }
    }
}

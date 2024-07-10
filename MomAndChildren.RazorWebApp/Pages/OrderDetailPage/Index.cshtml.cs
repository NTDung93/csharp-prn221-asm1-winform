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
    public class IndexModel : PageModel
    {
        private readonly IOrderDetailBusiness _orderDetail;

        public IndexModel(IOrderDetailBusiness orderDetail)
        {
            _orderDetail = orderDetail;
        }

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public string? Description { get; set; }

        public IEnumerable<OrderDetail> OrderDetail { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string searchTerm = "")
        {
            var result = await _orderDetail.GetOrderDetailsAsync();

            //Paging
            CurrentPage = currentPage;
            int pageSize = 5;
            var orderDetail = await _orderDetail.GetOrderDetailsAsync();

            //Search
            if (result != null && result.Status > 0 && result.Data != null)
            {
                OrderDetail = orderDetail.Data as List<OrderDetail>;
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = await _orderDetail.SearchByProductName(searchTerm);
                OrderDetail = result.Data as List<OrderDetail>;

                return Page();
            }


            int totalOrderDetail = OrderDetail.Count();
            TotalPages = (int)Math.Ceiling((double)totalOrderDetail / pageSize);

            OrderDetail = OrderDetail.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return Page();
        }
    }
}

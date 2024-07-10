using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.OrderDetailPage
{
    public class DeleteModel : PageModel
    {
        private readonly IOrderDetailBusiness _orderDetail;
        private readonly IProductBusiness _product;
        private readonly IOrderBusiness _order;

        public DeleteModel(IOrderDetailBusiness orderDetail, IProductBusiness product, IOrderBusiness order)
        {
            _orderDetail = orderDetail;
            _product = product;
            _order = order;
        }

        [BindProperty]
        public OrderDetail OrderDetail { get; set; } = default!;
        [BindProperty]
        [Required]
        public int OrderDetailId { get; set; }

        [BindProperty]
        [Required]
        public double UnitPrice { get; set; }

        [BindProperty]
        [Required]
        public double TotalPrice { get; set; }

        [BindProperty]
        [Required]
        public string CreateBy { get; set; }
        [BindProperty]
        [Required]
        public DateTime CreateAt { get; set; }
        [BindProperty]
        [Required]
        public string UpdateBy { get; set; }
        [BindProperty]
        [Required]
        public DateTime UpdateAt { get; set; }

        [BindProperty]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be >0")]

        public int Quantity { get; set; }

        [BindProperty]
        [Required]
        public int OrderId { get; set; }
        [BindProperty]
        [Required]
        public int ProductId { get; set; }

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
                OrderDetailId = OrderDetail.OrderDetailId;
                OrderId = OrderDetail.OrderId;
                ProductId = OrderDetail.Product.ProductId;
                Quantity = OrderDetail.Quantity;
                UnitPrice = OrderDetail.UnitPrice;
                TotalPrice = OrderDetail.TotalPrice;
                CreateBy = OrderDetail.CreateBy;
                CreateAt = OrderDetail.CreateAt;
                UpdateBy = OrderDetail.UpdateBy;
                UpdateAt = OrderDetail.UpdateAt;
            }
            //OrderDetail = orderDetail.Result as OrderDetail;



            var selectListProducts = _product.GetProductsAsync().Result.Data as List<Product>;
            var selectListOrders = _order.GetOrdersAsync().Result.Data as List<Order>;
            ViewData["OrderId"] = new SelectList(selectListOrders, "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(selectListProducts, "ProductId", "ProductName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var orderdetail = _orderDetail.GetOrderDetailByIdAsync(id);

            if (orderdetail != null)
            {
                await _orderDetail.DeleteOrderDetail(id);
            }

            return RedirectToPage("./Index");
        }
    }
}

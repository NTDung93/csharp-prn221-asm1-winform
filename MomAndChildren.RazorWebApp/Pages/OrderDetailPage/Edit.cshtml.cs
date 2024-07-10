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
    public class EditModel : PageModel
    {
        private readonly IOrderDetailBusiness _orderDetail;
        private readonly IProductBusiness _product;
        private readonly IOrderBusiness _order;

        public EditModel(IOrderDetailBusiness orderDetail, IProductBusiness product, IOrderBusiness order)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var selectListProducts = _product.GetProductsAsync().Result.Data as List<Product>;
                var selectListOrders = _order.GetOrdersAsync().Result.Data as List<Order>;
                ViewData["OrderId"] = new SelectList(selectListOrders, "OrderId", "OrderId");
                ViewData["ProductId"] = new SelectList(selectListProducts, "ProductId", "ProductName");
                return Page();
            }
            var orderDetailUpdated = new OrderDetail();
            orderDetailUpdated.OrderDetailId = OrderDetailId;
            orderDetailUpdated.OrderId = OrderId;
            orderDetailUpdated.ProductId = ProductId;
            orderDetailUpdated.Quantity = Quantity;
            orderDetailUpdated.UnitPrice = UnitPrice;
            orderDetailUpdated.TotalPrice = TotalPrice;
            orderDetailUpdated.CreateBy = CreateBy;
            orderDetailUpdated.CreateAt = CreateAt;
            orderDetailUpdated.UpdateBy = UpdateBy;
            orderDetailUpdated.UpdateAt = UpdateAt;

            await _orderDetail.UpdateOrderDetail(orderDetailUpdated);
            return RedirectToPage("./Index");
        }

        //public async Task<IActionResult> OnGetGetProductPrice(int productId)
        //{
        //    var product = _product.GetProductByIdAsync(productId).Result.Data as Product; // Thay thế bằng phương thức lấy sản phẩm từ service hoặc repository của bạn
        //    if (product != null)
        //    {
        //        return new JsonResult(product.Price); // Trả về giá sản phẩm dưới dạng JsonResult
        //    }
        //    return NotFound();
        //}
    }
}

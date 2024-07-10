using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.OrderDetailPage
{
    public class CreateModel : PageModel
    {
        private readonly IOrderDetailBusiness _orderDetail;
        private readonly IProductBusiness _product;
        private readonly IOrderBusiness _order;

        public CreateModel(IOrderDetailBusiness orderDetail, IProductBusiness product, IOrderBusiness order)
        {
            _orderDetail = orderDetail;
            _product = product;
            _order = order;
        }

        public IActionResult OnGet()
        {
            var selectListProducts = _product.GetProductsAsync().Result.Data as List<Product>;
            var selectListOrders = _order.GetOrdersAsync().Result.Data as List<Order>;
            ViewData["OrderId"] = new SelectList(selectListOrders, "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(selectListProducts, "ProductId", "ProductName");
            return Page();
        }

        [BindProperty]
        public OrderDetail OrderDetail { get; set; } = default!;
        [BindProperty]
        [Required]
        public int OrderDetailId { get; set; }
        [BindProperty]
        [Required]
        public float UnitPrice { get; set; }

        [BindProperty]
        [Required]
        public float TotalPrice { get; set; }

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


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                var selectListItems = _orderDetail.GetOrderDetailsAsync().Result.Data as List<OrderDetail>;
                //ViewData["OrderId"] = new SelectList(selectListItems, "OrderId", "OrderId");
                //ViewData["ProductId"] = new SelectList(selectListItems, "ProductId", "ProductName");
                return Page();
            }
            var orderDetailCreated = new OrderDetail();
            orderDetailCreated.OrderId = OrderId;
            orderDetailCreated.ProductId = ProductId;
            orderDetailCreated.Quantity = Quantity;
            orderDetailCreated.UnitPrice = UnitPrice;
            orderDetailCreated.TotalPrice = TotalPrice;
            orderDetailCreated.CreateBy = CreateBy;
            orderDetailCreated.CreateAt = CreateAt;
            orderDetailCreated.UpdateBy = UpdateBy; 
            orderDetailCreated.UpdateAt = UpdateAt;
            await _orderDetail.CreateOrderDetail(orderDetailCreated);
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetGetProductPrice(int productId)
        {
            var product = _product.GetProductByIdAsync(productId).Result.Data as Product; // Thay thế bằng phương thức lấy sản phẩm từ service hoặc repository của bạn
            if (product != null)
            {
                return new JsonResult(product.Price); // Trả về giá sản phẩm dưới dạng JsonResult
            }
            return NotFound();
        }
    }
}

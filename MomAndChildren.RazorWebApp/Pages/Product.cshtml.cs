using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;
using MomAndChildren.Data.Repositories;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace MomAndChildren.RazorWebApp.Pages
{
    public class ProductModel : PageModel
    {
        private readonly IProductBusiness _productBusiness = new ProductBusiness();
        private Net1710_221_3_MomAndChildrenContext _context = new Net1710_221_3_MomAndChildrenContext();
        public string Message { get; set; } = default;
        [BindProperty]
        public Product Product { get; set; } = default;
        public List<Product> Products { get; set; } = new List<Product>();

        [BindProperty]
        public string ProductName { get; set; }

        [BindProperty]
        public int Status { get; set; }

        [BindProperty]
        public DateTime ExpireDate { get; set; }

        [BindProperty]
        public DateTime ManufacturingDate { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        [BindProperty]
        public double Price { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public int BrandId { get; set; }

        [BindProperty]
        public int CategoryId { get; set; }

        public void OnGet()
        {
            Products = this.GetProducts();
        }

        public IActionResult OnPostAddProduct()
        {
            var product = new Product();
            product.ProductName = ProductName;
            product.Status = Status;
            product.ExpireDate = ExpireDate;
            product.ManufacturingDate = ManufacturingDate;
            product.Quantity = Quantity;
            product.Price = Price;
            product.Description = Description;
            product.BrandId = BrandId;
            product.CategoryId = CategoryId;

            _context.Add(product);
            _context.SaveChangesAsync();

            return RedirectToPage("./Product");
        }

        private List<Product> GetProducts()
        {
            var productsResult = _productBusiness.GetProductsWithNestedObjAsync();
            if (productsResult != null)
            {
                return (List<Product>)productsResult.Result.Data;
            }
            return new List<Product>();
        }

        public IActionResult OnPostDelete(int productId)
        {
            var ProductResult = _productBusiness.GetProductByIdAsync(productId);

            if (ProductResult != null)
            {
                _productBusiness.DeleteProduct(productId);
            }

            return Redirect("Product");
        }
    }
}
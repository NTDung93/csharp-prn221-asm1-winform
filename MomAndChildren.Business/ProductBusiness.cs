using MomAndChildren.Common;
using MomAndChildren.Data;
using MomAndChildren.Data.Models;
using MomAndChildren.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Business
{
    public interface IProductBusiness
    {
        Task<IMomAndChildrenResult> GetProductsAsync();
        Task<IMomAndChildrenResult> GetProductsWithNestedObjAsync();
        Task<IMomAndChildrenResult> GetProductByIdAsync(int id);
        Task<IMomAndChildrenResult> GetProductByIdWithNestedObjAsync(int id);
        Task<IMomAndChildrenResult> CreateProduct(Product product);
        Task<IMomAndChildrenResult> UpdateProduct(Product product);
        Task<IMomAndChildrenResult> DeleteProduct(int productId);
        Task<IMomAndChildrenResult> SearchByKeyword(string? searchTerm);
        bool ProductExists(int id);
    }

    public class ProductBusiness : IProductBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        private readonly ProductRepository ProductRepository;

        public ProductBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
            ProductRepository = new ProductRepository();
        }

        public async Task<IMomAndChildrenResult> CreateProduct(Product product)
        {
            if (_unitOfWork.ProductRepository.GetAll().Any(p => p.ProductId == product.ProductId))
            {
                return new MomAndChildrenResult(-1, "Product id is duplicate");
            }
            await _unitOfWork.ProductRepository.CreateAsync(product);
            return new MomAndChildrenResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
        }

        public async Task<IMomAndChildrenResult> DeleteProduct(int productId)
        {
            Product product = _unitOfWork.ProductRepository.GetById(productId);
            if (product == null)
            {
                return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
            }
            else
            {
                await _unitOfWork.ProductRepository.RemoveAsync(product);
                return new MomAndChildrenResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }

        public async Task<IMomAndChildrenResult> GetProductByIdAsync(int productId)
        {
            Product product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
            }
            else
            {
                return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, product);
            }
        }

        public async Task<IMomAndChildrenResult> GetProductsAsync()
        {
            List<Product> products = await _unitOfWork.ProductRepository.GetAllAsync();
            if (products == null)
            {
                return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
            return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, products);
        }

        public async Task<IMomAndChildrenResult> GetProductsWithNestedObjAsync()
        {
            var products = await _unitOfWork.ProductRepository.getProductsAsync();
            if (products == null)
            {
                return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
            return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, products);
        }

        public bool ProductExists(int id)
        {
            return _unitOfWork.ProductRepository.GetAll().Any(e => e.ProductId == id);
        }

        public async Task<IMomAndChildrenResult> UpdateProduct(Product product)
        {
            Product product1 = await _unitOfWork.ProductRepository.GetByIdAsync(product.ProductId);
            if (product1 == null)
            {
                return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
            }
            else
            {
                product1.ProductName = product.ProductName;
                product1.Description = product.Description;
                product1.Price = product.Price;
                product1.Quantity = product.Quantity;
                product1.CategoryId = product.CategoryId;
                product1.BrandId = product.BrandId;
                product1.Status = product.Status;
                product1.ManufacturingDate = product.ManufacturingDate;
                product1.ExpireDate = product.ExpireDate;
                await _unitOfWork.ProductRepository.UpdateAsync(product1);
            }
            return new MomAndChildrenResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
        }

        public async Task<IMomAndChildrenResult> SearchByKeyword(string? searchTerm)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
                var result = products.Where(c => c.ProductName.ToLower().Contains(searchTerm.ToLower())
                    || c.Description.ToLower().Contains(searchTerm.ToLower())
                    ).ToList();
                return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> GetProductByIdWithNestedObjAsync(int id)
        {
            Product products = ProductRepository.getProductByIdAsync(id);
            if (products == null)
            {
                return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
            return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, products);
        }
    }
}

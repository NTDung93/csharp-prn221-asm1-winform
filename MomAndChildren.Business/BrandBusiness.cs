using Microsoft.EntityFrameworkCore;
using MomAndChildren.Common;
using MomAndChildren.Data;
using MomAndChildren.Data.DAO;
using MomAndChildren.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Business
{
    public interface IBrandBusiness
    {
        Task<IMomAndChildrenResult> GetBrandsAsync();
        Task<IMomAndChildrenResult> GetBrandByIdAsync(int brandId);
        Task<IMomAndChildrenResult> CreateBrand(Brand brand);
        Task<IMomAndChildrenResult> UpdateBrand(Brand brand);
        Task<IMomAndChildrenResult> DeleteBrand(int brandId);
        Task<IMomAndChildrenResult> ChangeStatus(int brandId);
        Task<IMomAndChildrenResult> SearchByKeyword(string? searchTerm);
    }

    public class BrandBusiness : IBrandBusiness
    {
        //private readonly BrandDAO _BrandDAO;
        //private readonly ProductDAO _ProductDAO;
        private readonly UnitOfWork _unitOfWork;

        public BrandBusiness()
        {
            //_BrandDAO = new BrandDAO();
            //_ProductDAO = new ProductDAO();
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IMomAndChildrenResult> CreateBrand(Brand brand)
        {
            try
            {
                //check trung ten
                var brands = await _unitOfWork.BrandRepository.GetAllAsync();
                foreach (var item in brands)
                {
                    if (brand.BrandName.Equals(item.BrandName))
                    {
                        return new MomAndChildrenResult(Const.ERROR_EXCEPTION, "Name is duplicated.");
                    }
                }
                int result = await _unitOfWork.BrandRepository.CreateAsync(brand);
                if (result > 0)
                {
                    return new MomAndChildrenResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }

        }

        public async Task<IMomAndChildrenResult> DeleteBrand(int brandId)
        {
            try
            {
                var brand = await _unitOfWork.BrandRepository.GetByIdAsync(brandId);
                if (brand != null)
                {
                    //xem bang product co brand muon xóa ko, có -> thì ko cho xóa; ko có -> chuyển status
                    var products = await _unitOfWork.ProductRepository.GetAllAsync();
                    foreach (var product in products)
                    {
                        if (brandId == product.BrandId)
                        {
                            return new MomAndChildrenResult(Const.ERROR_EXCEPTION, "Brand is using");
                        }
                    }
                    bool result = await _unitOfWork.BrandRepository.RemoveAsync(brand);
                    if (result)
                    {
                        return new MomAndChildrenResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                    }
                    else
                    {
                        return new MomAndChildrenResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                    }
                }
                else
                {
                    return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> ChangeStatus(int brandId)
        {
            try
            {
                var brand = await _unitOfWork.BrandRepository.GetByIdAsync(brandId);
                if (brand != null)
                {
                    if (brand.Status == 0)
                    {
                        brand.Status = 1;
                        int result = await _unitOfWork.BrandRepository.UpdateAsync(brand);
                        if (result > 0)
                        {
                            return new MomAndChildrenResult(Const.SUCCESS_UPDATE_CODE, "Brand is active");
                        }
                        else
                        {
                            return new MomAndChildrenResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                        }
                    }
                    else
                    {
                        brand.Status = 0;
                        int result = await _unitOfWork.BrandRepository.UpdateAsync(brand);
                        if (result > 0)
                        {
                            return new MomAndChildrenResult(Const.SUCCESS_UPDATE_CODE, "Brand is inactive");
                        }
                        else
                        {
                            return new MomAndChildrenResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                        }
                    }
                }
                else
                {
                    return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> GetBrandsAsync()
        {
            try
            {
                var brands = await _unitOfWork.BrandRepository.GetAllAsync();

                if (brands == null)
                {
                    return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, brands);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> GetBrandByIdAsync(int brandId)
        {
            try
            {
                var brand = await _unitOfWork.BrandRepository.GetByIdAsync(brandId);
                if (brand == null)
                {
                    return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, brand);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> UpdateBrand(Brand brand)
        {
            try
            {
                var newBrand = await _unitOfWork.BrandRepository.GetByIdAsync(brand.BrandId);
                if (newBrand != null)
                {
                    newBrand.BrandName = brand.BrandName;
                    newBrand.Status = brand.Status;
                    newBrand.Description = brand.Description;
                    newBrand.Image = brand.Image;
                    newBrand.Note = brand.Note;
                    newBrand.CreateBy = brand.CreateBy;
                    newBrand.CreateAt = brand.CreateAt;
                    newBrand.UpdateBy = brand.UpdateBy;
                    newBrand.UpdateAt = brand.UpdateAt;
                    int result = await _unitOfWork.BrandRepository.UpdateAsync(newBrand);
                    if (result > 0)
                    {
                        return new MomAndChildrenResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                    }
                    else
                    {
                        return new MomAndChildrenResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                    }
                }
                else
                {
                    return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> SearchByKeyword(string? searchTerm)
        {
            try
            {
                var brands = await _unitOfWork.BrandRepository.GetAllAsync();
                var result = brands.Where(c => c.BrandName.ToLower().Contains(searchTerm.ToLower())
                    || c.Description.ToLower().Contains(searchTerm.ToLower())
                    || c.Note.ToLower().Contains(searchTerm.ToLower())
                    || c.CreateBy.ToLower().Contains(searchTerm.ToLower())
                    || c.UpdateBy.ToLower().Contains(searchTerm.ToLower())
                    ).ToList();
                return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }

        }
    }
}

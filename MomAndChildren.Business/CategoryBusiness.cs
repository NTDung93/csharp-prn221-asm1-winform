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
    public interface ICategoryBusiness
    {
        Task<IMomAndChildrenResult> GetCategoriesAsync();
        Task<IMomAndChildrenResult> GetCategoryByIdAsync(int categoryId);
        Task<IMomAndChildrenResult> CreateCategory(Category category);
        Task<IMomAndChildrenResult> UpdateCategory(Category category);
        Task<IMomAndChildrenResult> DeleteCategory(int categoryId);
        Task<IMomAndChildrenResult> ChangeStatus(int categoryId);
        Task<IMomAndChildrenResult> SearchByKeyword(string? searchTerm);
    }

    public class CategoryBusiness : ICategoryBusiness
    {
        //private readonly CategoryDAO _CategoryDAO;
        //private readonly ProductDAO _ProductDAO;
        private readonly UnitOfWork _unitOfWork;

        public CategoryBusiness()
        {
            //_CategoryDAO = new CategoryDAO();
            //_ProductDAO = new ProductDAO();
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IMomAndChildrenResult> CreateCategory(Category category)
        {
            try
            {
                //check trung ten
                //var categories = await _context.Categories.OrderByDescending(c => c.CategoryName).ToListAsync();
                //var categories = await _CategoryDAO.GetAllAsync();
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
                foreach (var item in categories)
                {
                    if (category.CategoryName.Equals(item.CategoryName))
                    {
                        return new MomAndChildrenResult(Const.ERROR_EXCEPTION, "Name is duplicated.");
                    }
                }
                int result = await _unitOfWork.CategoryRepository.CreateAsync(category);

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

        public async Task<IMomAndChildrenResult> DeleteCategory(int categoryId)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
                if (category != null)
                {     
                    //xem bang product co category muon xóa ko, có -> thì ko cho xóa; ko có -> chuyển status
                    var products = await _unitOfWork.ProductRepository.GetAllAsync();
                    foreach (var product in products)
                    {
                        if (categoryId == product.CategoryId)
                        {
                            return new MomAndChildrenResult(Const.ERROR_EXCEPTION, "Category is using");
                        }
                    }

                    bool result = await _unitOfWork.CategoryRepository.RemoveAsync(category);
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

        public async Task<IMomAndChildrenResult> ChangeStatus(int categoryId)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
                if (category != null)
                {
                    if (category.Status == 0)
                    {
                        category.Status = 1;
                        int result = await _unitOfWork.CategoryRepository.UpdateAsync(category);
                        if (result > 0) 
                        {
                            return new MomAndChildrenResult(Const.SUCCESS_UPDATE_CODE, "Category is active");
                        }
                        else
                        {
                            return new MomAndChildrenResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                        }
                    }
                    else
                    {
                        category.Status = 0;
                        int result = await _unitOfWork.CategoryRepository.UpdateAsync(category);
                        if (result > 0)
                        {
                            return new MomAndChildrenResult(Const.SUCCESS_UPDATE_CODE, "Category is inactive");
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

        public async Task<IMomAndChildrenResult> GetCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

                if (categories == null)
                {
                    return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, categories);
                }
            } 
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, category);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> UpdateCategory(Category category)
        {
            try
            {
                var newCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(category.CategoryId);
                if (newCategory != null)
                {        
                    newCategory.CategoryName = category.CategoryName;
                    newCategory.Description = category.Description;
                    newCategory.Status = category.Status;
                    newCategory.Image = category.Image;
                    newCategory.Note = category.Note;
                    newCategory.CreateBy = category.CreateBy;
                    newCategory.CreateAt = category.CreateAt;
                    newCategory.UpdateBy = category.UpdateBy;
                    newCategory.UpdateAt = category.UpdateAt;
                    int result = await _unitOfWork.CategoryRepository.UpdateAsync(newCategory);
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
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
                var result = categories.Where(c => c.CategoryName.ToLower().Contains(searchTerm.ToLower()) 
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

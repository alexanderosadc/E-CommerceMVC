using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin;
using Ebay.Infrastructure.ViewModels.Admin.CreateCategory;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Infrastructure.Helpers;

using Microsoft.AspNetCore.Mvc.Rendering;
using Ebay.Infrastructure.Interfaces.AdminPresentation;

namespace Ebay.Infrastructure.Business_Logic
{
    public class CategoryBusinessLogic : ICategoryBL
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryBusinessLogic(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryViewDTO>> GetCategoyDTO()
        {
            var categories = await _categoryRepository.GetAll();
            var categoryViews = categories.Select(item => CreateCategoryView(item));
            return categoryViews;
        }

        public CategoryViewDTO CreateCategoryView(Category category)
        {
            var categoryViewModel = new CategoryViewDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ChildCategories = category.Categories.Select(item => item.Name).ToList()
            };
            if(category.Parent != null)
            {
                categoryViewModel.ParentName = category.Parent.Name;
            }
            return categoryViewModel;
        }

        public async Task<CategoryCreateDTO> GetCategoryCreateDTO()
        {
            CategoryCreateDTO categoryCreateViewModel = new CategoryCreateDTO();
            var lastEntity = await _categoryRepository.GetLastItem();
            categoryCreateViewModel.Id = lastEntity.Id + 1;
            var productCategories = await _categoryRepository.GetAll();
            categoryCreateViewModel.AllChildrenCategories = await DropdownHelper.CreateDropdownCategory(productCategories);


            return categoryCreateViewModel;
        }

        public async Task CreateNewCategory(CategoryCreateDTO categoryViewModel)
        {
            List<Category> childCategories = await GetChildCategories(categoryViewModel);
            Category category = DTOMapper.ToCategory(categoryViewModel, childCategories, isNew: true);
            await _categoryRepository.Insert(category);
        }

        public async Task<CategoryCreateDTO> EditCategory(int itemId)
        {
            var category = await _categoryRepository.Get(itemId);
            var productCategories = await _categoryRepository.GetAll();
            var productCategoriesWithoutCurrent = productCategories
                .Where(item => item.Id != category.Id)
                .ToList();
            var categoryCreateModel = await DTOMapper.ToCategoryCreateDTO(category, productCategoriesWithoutCurrent);
            return categoryCreateModel;
        }

        public async Task UpdateCategory(CategoryCreateDTO categoryCreateViewModel)
        {
            List<Category> childCategories = await GetChildCategories(categoryCreateViewModel);
           
            var category = DTOMapper.ToCategory(categoryCreateViewModel, childCategories, isNew: false);
            await _categoryRepository.Update(category);
        }

        public async Task Delete(string itemId)
        {
            var category = await _categoryRepository.Get(int.Parse(itemId));
            await _categoryRepository.Delete(category);
        }

        public async Task<List<Category>> GetChildCategories(CategoryCreateDTO dto)
        {
            var childCategories = new List<Category>();
            if (dto.ChildIds != null)
            {
                foreach (var childId in dto.ChildIds)
                {
                    var childCategory = await _categoryRepository.Get(childId);
                    childCategories.Add(childCategory);
                }
            }
            return childCategories;
        }

        public async Task<List<SelectListItem>> GetDropdownCategories()
        {
            var categories = await _categoryRepository.GetAll();
            return await DropdownHelper.CreateDropdownCategory(categories);
        }
    } 
} 

using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin;
using Ebay.Infrastructure.ViewModels.Admin.CreateCategory;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Presentation.Helpers;
using Ebay.Presentation.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ebay.Presentation.Business_Logic
{
    public class CategoryBusinessLogic
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly CategoryService _categoryService;
        public CategoryBusinessLogic(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryService = new CategoryService(categoryRepository);
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
            categoryCreateViewModel.Id = await _categoryService.GetNumberOfRecords() + 1;
            //categoryCreateViewModel.AllParentCategories = await _categoryService.CreateDropdownCategory();
            /*categoryCreateViewModel.AllParentCategories.Add(new SelectListItem
            {
                Text = "None",
                Value = null
            });*/
            var productCategories = await _categoryRepository.GetAll();
            categoryCreateViewModel.AllChildrenCategories = await DropdownHelper.CreateDropdownCategory(productCategories);


            return categoryCreateViewModel;
        }

        public async Task CreateNewCategory(CategoryCreateDTO categoryViewModel)
        {
            List<Category> childCategories = await GetChildCategories(categoryViewModel);
            Category category = DTOMapper.ToCategory(categoryViewModel, childCategories);
            await _categoryRepository.Insert(category);
        }

        public async Task<CategoryCreateDTO> EditCategory(int itemId)
        {
            var category = await _categoryRepository.Get(itemId);
            var productCategories = await _categoryRepository.GetAll();
            var categoryCreateModel = await DTOMapper.ToCategoryCreateDTO(category, productCategories);
            return categoryCreateModel;
        }

        public async Task UpdateCategory(CategoryCreateDTO categoryCreateViewModel)
        {
            List<Category> childCategories = await GetChildCategories(categoryCreateViewModel);
            var category = DTOMapper.ToCategory(categoryCreateViewModel, childCategories);
            await _categoryRepository.Update(category);
        }

        public async Task DeleteCategory(int itemId)
        {
            var category = await _categoryRepository.Get(itemId);
            await _categoryRepository.Delete(category);
        }

        private async Task<List<Category>> GetChildCategories(CategoryCreateDTO dto)
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
    } 
} 

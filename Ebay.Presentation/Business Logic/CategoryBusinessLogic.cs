using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin;
using Ebay.Infrastructure.ViewModels.Admin.CreateCategory;
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

        public async Task<IEnumerable<CategoryViewModel>> GetCategoyDTO()
        {
            var categories = await _categoryRepository.GetAll();
            var categoryViews = categories.Select(item => CreateCategoryView(item));
            return categoryViews;
        }

        public CategoryViewModel CreateCategoryView(Category category)
        {
            var categoryViewModel = new CategoryViewModel
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

        public async Task<CategoryCreateViewModel> GetCategoryCreateDTO()
        {
            CategoryCreateViewModel categoryCreateViewModel = new CategoryCreateViewModel();
            categoryCreateViewModel.Id = await _categoryService.GetNumberOfRecords() + 1;
            //categoryCreateViewModel.AllParentCategories = await _categoryService.CreateDropdownCategory();
            /*categoryCreateViewModel.AllParentCategories.Add(new SelectListItem
            {
                Text = "None",
                Value = null
            });*/
            categoryCreateViewModel.AllChildrenCategories = await _categoryService.CreateDropdownCategory();


            return categoryCreateViewModel;
        }

        public async Task CreateNewCategory(CategoryCreateViewModel categoryViewModel)
        {
            Category category = await _categoryService.FromCreateDtoToCategory(categoryViewModel);
            await _categoryRepository.Insert(category);
        }

        public async Task<CategoryCreateViewModel> EditCategory(int itemId)
        {
            var category = await _categoryRepository.Get(itemId);
            var categoryCreateModel = await _categoryService.FromCategoryToCreateDto(category);
            return categoryCreateModel;
        }

        public async Task UpdateCategory(CategoryCreateViewModel categoryCreateViewModel)
        {
            var product = await _categoryService.FromCreateDtoToCategory(categoryCreateViewModel);
            await _categoryRepository.Update(product);
        }

        public async Task DeleteCategory(int itemId)
        {
            var category = await _categoryRepository.Get(itemId);
            await _categoryRepository.Delete(category);
        }
    } 
} 

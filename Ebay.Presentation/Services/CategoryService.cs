using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin.CreateCategory;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ebay.Presentation.Services
{
    public class CategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        /// <summary>
        ///  Method <c>CreateDropdownCategory</c> gets the <c>List<SelectedListItem></c> for visualization 
        ///  dropdown menu in UI.
        /// </summary>
        /// <returns>
        ///     <c>List of SelectedListItems.</c> entity.
        /// </returns>
        public async Task<List<SelectListItem>> CreateDropdownCategory()
        {

            var productCategories = await _categoryRepository.GetAll();

            return productCategories.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            }).ToList(); ;
        }
        /// <summary>
        ///  Method <c>GetSelectedCategories</c> gets <c>ProductCreateViewModel</c> and returns 
        ///  all categories which are related to this product.
        /// </summary>
        /// <param name="viewModel">
        ///     ProductCreateViewModel which is retrieved from UI.
        /// </param>
        /// <returns>
        ///     <c>List<Category></c> entity, which represents all categories related to the product.
        /// </returns>
        public List<Category> GetSelectedCategories(ProductCreateViewModel viewModel)
        {
            return viewModel.CategoriesIds
                .Select(async item => await _categoryRepository.Get(item))
                .Select(task => task.Result)
                .Where(category => category != null)
                .ToList();
        }

        public async Task<int> GetNumberOfRecords()
        {
            var categories = await _categoryRepository.GetAll();
            return categories.AsQueryable().Count();
        }

        public async Task<Category> FromCreateDtoToCategory(CategoryCreateViewModel categoryViewModel)
        {
            var category = new Category
            {
                Id = categoryViewModel.Id,
                Name = categoryViewModel.Name,
                Description = categoryViewModel.Description,
            };
            var childCategories = new List<Category>();
            if(categoryViewModel.ChildIds != null)
            {
                foreach (var childId in categoryViewModel.ChildIds)
                {
                    var childCategory = await _categoryRepository.Get(childId);
                    childCategories.Add(childCategory);
                }
            }
            

            category.Categories = childCategories;
            return category;
        }

        public async Task<CategoryCreateViewModel> FromCategoryToCreateDto(Category category)
        {
            List<int> selectedItemsId = new List<int>();
            if (category.Categories != null)
            {
                selectedItemsId = category.Categories.Select(category => category.Id).ToList();
            }
            
            var categoryCreateView = new CategoryCreateViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                AllChildrenCategories = await CreateDropdownCategory()
            };

            categoryCreateView.AllChildrenCategories
                .ForEach(selectedElement => selectedElement.Selected =
                selectedItemsId.Contains(int.Parse(selectedElement.Value)));

            return categoryCreateView;
        }
    }
}

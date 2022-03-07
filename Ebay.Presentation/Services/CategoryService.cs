using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;
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

        public async Task<List<SelectListItem>> CreateDropdownCategory()
        {

            var productCategories = await _categoryRepository.GetAll();
            var categorySelectedItems = productCategories.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            });

            return categorySelectedItems.ToList();
        }

        public List<Category> GetSelectedCategories(ProductCreateViewModel viewModel)
        {
            return viewModel.CategoriesIds
                .Select(async item => await _categoryRepository.Get(item))
                .Select(task => task.Result)
                .Where(category => category != null)
                .ToList();
        }
    }
}

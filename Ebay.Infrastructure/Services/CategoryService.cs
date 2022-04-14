using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Interfaces.AdminPresentation.Services;
using Ebay.Infrastructure.ViewModels.Admin.CreateCategory;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ebay.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
        public List<Category> GetSelectedCategories(ProductCreateDTO viewModel)
        {
            return viewModel.CategoriesIds
                .Select(async item => await _categoryRepository.Get(item))
                .Select(task => task.Result)
                .Where(category => category != null)
                .ToList();
        }
    }
}

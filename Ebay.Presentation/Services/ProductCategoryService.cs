using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Interfaces.Services;

namespace Ebay.Presentation.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private IRepository<ProductCategory> _productCategoryRepository;
        public ProductCategoryService(IRepository<ProductCategory> productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        /// <summary>
        ///  Method <c>CreateProductCategories</c> creates product categories for new relation between 
        ///  <c>Product</c> entity and <c>Category</c>.
        /// </summary>
        /// <param name="categories">
        ///     List of all categories which user wants to be related to the product
        /// </param>
        /// <param name="product">
        ///     <c>Product entity</c>> which was created or updated
        /// </param>
        /// <returns>
        ///     <c>List<ProductCategory></c> which is the list of all new relation between product and category.
        /// </returns>
        public List<ProductCategory> CreateProductCategories(List<Category> categories, Product product)
        {
            var productCategories = categories
                .Select(category => new ProductCategory
                {
                    CategoryId = category.Id,
                    Category = category,
                    ProductId = product.Id,
                    Product = product
                });
            return productCategories.ToList();
        }

        /// <summary>
        ///  Method <c>DeleteAll</c> deletes all relations between the specific <c>Product</c> entity and categories. 
        /// </summary>
        /// <param name="productId">
        ///     Represents the Id of the product relations of what we want to delete.
        /// </param>
        public async Task DeleteAll(int productId)
        {
            var allCategories = await _productCategoryRepository.GetAll();
            var productCategories = allCategories.Where(item => item.ProductId == productId).ToList();
            foreach (var item in productCategories)
            {
                await _productCategoryRepository.Delete(item);
            }
            // Not working gives an error
            //productCategories.ForEach(async productCategory => await _productCategoryRepository.Delete(productCategory));
        }
    }
}

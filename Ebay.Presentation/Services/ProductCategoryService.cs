using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;

namespace Ebay.Presentation.Services
{
    public class ProductCategoryService
    {
        private IRepository<ProductCategory> _productCategoryRepository;
        public ProductCategoryService(IRepository<ProductCategory> productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

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
            /*foreach (var item in productCategories)
            {
                _productCategoryRepository.Insert(item);
            }*/
            return productCategories.ToList();
        }

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

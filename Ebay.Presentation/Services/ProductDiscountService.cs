using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;

namespace Ebay.Presentation.Services
{
    public class ProductDiscountService
    {
        public readonly IRepository<ProductDiscount> _productDiscountRepository;
        public ProductDiscountService(IRepository<ProductDiscount> productRepository)
        {
            _productDiscountRepository = productRepository;
        }

        public List<ProductDiscount> CreateProductDiscounts(List<Discount> discounts, Product product)
        {
            var productCategories = discounts
                .Select(discount => new ProductDiscount
                {
                    DiscountId = discount.Id,
                    Discount = discount,
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
            var allDiscounts = await _productDiscountRepository.GetAll();
            var productDiscoutns = allDiscounts.Where(item => item.ProductId == productId).ToList();
            foreach (var item in productDiscoutns)
            {
                await _productDiscountRepository.Delete(item);
            }
            // Not working gives an error
            //productCategories.ForEach(async productCategory => await _productCategoryRepository.Delete(productCategory));
        }
    }
}

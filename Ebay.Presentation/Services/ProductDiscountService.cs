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

        public List<ProductDiscount> GetProductDiscounts(List<Discount> discounts, Product product)
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
    }
}

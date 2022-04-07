using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Interfaces.Services;

namespace Ebay.Presentation.Services
{
    public class ProductDiscountService : IProductDiscountService
    {
        public readonly IRepository<ProductDiscount> _productDiscountRepository;
        public ProductDiscountService(IRepository<ProductDiscount> productRepository)
        {
            _productDiscountRepository = productRepository;
        }
        /// <summary>
        ///  Method <c>CreateProductDiscounts</c> creates product discounts for new relation between 
        ///  <c>Product</c> entity and <c>Discount</c> entities.
        /// </summary>
        /// <param name="discounts">
        ///     List of all discounts which user wants to be related to the product
        /// </param>
        /// <param name="product">
        ///     <c>Product entity</c>> which was created or updated
        /// </param>
        /// <returns>
        ///     <c>List<ProductDiscount></c> which is the list of all new relation between product and discounts.
        /// </returns>
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
        /// <summary>
        ///  Method <c>DeleteAll</c> deletes all relations between the specific <c>Product</c> entity 
        ///  and <c>Discount</c> entity. 
        /// </summary>
        /// <param name="productId">
        ///     Represents the Id of the product relations of what we want to delete.
        /// </param>
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

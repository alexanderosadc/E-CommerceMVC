using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Persistance;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ebay.Presentation.Services
{
    public class ProductService
    {
        private readonly IRepository<Product> _productRepository;
        public ProductService(
            IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        ///  Method <c>GetNumberOfRecords</c> gets number of all records in the <c>Product</c> table in the database.
        /// </summary>
        /// <returns>
        ///     Number of elements in the <c>Product</c> table
        /// </returns>
        public async Task<int> GetNumberOfRecords()
        {
            var products = await _productRepository.GetAll();
            return products.AsQueryable().Count();
        }
        /// <summary>
        ///  Method <c>CreateProduct</c> creates <c>Product</c> from <c>ProductViewModel</c>.
        /// </summary>
        /// <param name="viewModel">
        ///     <c>ProductCreateViewModel</c> represents view of the product.
        /// </param>
        /// <param name="ignoreID">
        ///     Helps us to detect if we Update the entity(ignoreId = false) or if we create new Product (ignoreId = true)
        /// </param>
        /// <returns>
        ///     New <c>Product</c> entity.
        /// </returns>
        public Product CreateProduct(ProductCreateViewModel viewModel, bool ignoreID)
        {
            var product = new Product 
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Quantity = viewModel.TotalQuantity,
                    Price = viewModel.Price,
                };
            if(ignoreID == false)
            {
                product.Id = viewModel.Id;
            }
            return product;
        }

        /// <summary>
        ///  Method <c>GetProductCreateViewModelById</c> transforms <c>Product</c> to <c>ProductCreateViewModel</c>.
        /// <param name="id">
        ///     Id of the product we want to transform to the view.
        /// </param>
        /// <returns>
        ///     <c>ProductCreateViewModel</c> which represents a view of the entity.
        /// </returns>
        public async Task<ProductCreateViewModel> GetProductCreateViewModelById(int id)
        {
            var product = await _productRepository.Get(id);
            var productCreateView = new ProductCreateViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                TotalQuantity = product.Quantity,
                Price = product.Price,
            };
            return productCreateView;
        }

        /// <summary>
        ///  Method <c>GetSelectedCategoriesId</c> gets all categories related to the product.
        /// </summary>
        /// <param name="productId">
        ///     Id of the current product.
        /// </param>
        /// <returns>
        ///     <c>List<int></c> represents the list of Id of categories which are related to the product.
        /// </returns>
        public async Task<List<int>> GetSelectedCategoriesId(int productId)
        {
            var product = await _productRepository.Get(productId);
            var ids = product.ProductCategories.Select(item => item.CategoryId);
            return ids.ToList();
        }

        /// <summary>
        ///  Method <c>GetSelectedDiscountId</c> gets all discounts related to the product.
        /// </summary>
        /// <param name="productId">
        ///     Id of the current product.
        /// </param>
        /// <returns>
        ///     <c>List<int></c> represents the list of Id of discounts which are related to the product.
        /// </returns>
        public async Task<List<int>> GetSelectedDiscountId(int productId)
        {
            var product = await _productRepository.Get(productId);
            var ids = product.ProductDiscounts.Select(item => item.DiscountId);
            return ids.ToList();
        }
    }
}

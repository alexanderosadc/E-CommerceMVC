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

        public async Task<int> GetNumberOfRecords()
        {
            var products = await _productRepository.GetAll();
            return products.AsQueryable().Count();
        }
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

        public async Task<List<int>> GetSelectedCategoriesId(int productId)
        {
            var product = await _productRepository.Get(productId);
            var ids = product.ProductCategories.Select(item => item.CategoryId);
            return ids.ToList();
        }

        public async Task<List<int>> GetSelectedDiscountId(int productId)
        {
            var product = await _productRepository.Get(productId);
            var ids = product.ProductDiscounts.Select(item => item.DiscountId);
            return ids.ToList();
        }
    }
}

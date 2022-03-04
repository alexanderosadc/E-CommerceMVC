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

        public async Task CreateProduct(ProductCreateViewModel viewModel)
        {
            await _productRepository.Insert(
                new Product 
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Quantity = viewModel.TotalQuantity,
                    Price = viewModel.Price,
                });
        }

        public async Task<List<string>> GetCurrentProductCategoryNames(int productId)
        {
            IEnumerable<string> categoryNames = new List<string>();
            var product = await _productRepository.Get(productId);
                if(product != null)
                {
                    if (productId == product.Id)
                    {
                        categoryNames = product.ProductCategories.Select(item => item.Category.Name);
                    }
                }

            return categoryNames.ToList();
        }

        public async Task<List<Discount>> GetCurrentProductDiscounts(int productId)
        {
            IEnumerable<Discount> discounts = new List<Discount>();
            var product = await _productRepository.Get(productId);
            if (product != null)
            {
                if (productId == product.Id)
                {
                    discounts = product.ProductDiscounts.Select(item => item.Discount);
                }
            }

            return discounts.ToList();
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsViewModel()
        {
            var products = await _productRepository.GetAll();

            var productViews = new List<ProductViewModel>();

            foreach (var item in products)
            {
                if(item != null)
                {
                    productViews.Add(new ProductViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        TotalQuantity = item.Quantity,
                        Price = item.Price,
                    });
                }
            }
            return productViews;
        }

        public async Task<IEnumerable<ProductCreateViewModel>> GetAllProductsCreateViewModel()
        {
            var products = await _productRepository.GetAll();

            var productViews = products.Select(item => new ProductCreateViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                TotalQuantity = item.Quantity,
                Price = item.Price,
            });

            return productViews;
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
    }
}

using Ebay.Domain.Entities;
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
        private readonly IRepository<Discount> _discountRepository;
        private readonly IRepository<Category> _categoryRepository;
        public ProductService(
            IRepository<Product> productRepository,
            IRepository<Discount> discountRepository,
            IRepository<Category> productCategoryRepository)
        {
            _productRepository = productRepository;
            _discountRepository = discountRepository;
            _categoryRepository = productCategoryRepository;
        }

        public async Task CreateDropdownSelectedItems(ProductCreateViewModel productCreateViewModel)
        {
            productCreateViewModel.CategoryResponseItems = await CreateDropdownCategory();
            productCreateViewModel.DiscountItems = await CreateDropdownDiscounts();
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

        public async Task<List<SelectListItem>> CreateDropdownDiscounts()
        {

            var productCategories = await _discountRepository.GetAll();
            var categorySelectedItems = productCategories.Select(item => new SelectListItem
            {
                Text = item.Name + " = " + item.DiscountPercent.ToString() + "%",
                Value = item.Id.ToString()
            });

            return categorySelectedItems.ToList();
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

        private async Task<ICollection<Category>> ExtractCategories(List<string> listOfId, IRepository<Category> context)
        {
            ICollection<Category> categories = new List<Category>();
            if (listOfId != null)
            {
                foreach (var id in listOfId)
                {
                    var matchedCategory = await context.Get(int.Parse(id));
                    if(matchedCategory != null)
                    {
                        categories.Add(matchedCategory);
                    }
                }
            }
            return categories;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
        {
            var products = await _productRepository.GetAll();

            var productViews = new List<ProductViewModel>();
            foreach (var item in products)
            {
                if(item != null)
                {
                    productViews.Add(new ProductViewModel
                    {
                        Name = item.Name,
                        Description = item.Description,
                        TotalQuantity = item.Quantity,
                        Price = item.Price,
                        discountPercentages = await GetDiscountPercentage(),
                        CategoryNames = await GetCategoryNames()
                    });
                }
            }

            return productViews;
        }

        private async Task<List<int>> GetDiscountPercentage()
        {
            IEnumerable<Discount> discount = await _discountRepository.GetAll();
            List<int> discountPercentages = discount.Select(item => item.DiscountPercent).ToList();
            return discountPercentages;
        }

        private async Task<List<string>> GetCategoryNames()
        {
            IEnumerable<Category> category = await _categoryRepository.GetAll();
            List<string> categoryNames = category.Select(item => item.Name).ToList();
            return categoryNames;
        }
    }
}

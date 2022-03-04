using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Presentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ebay.Presentation.Business_Logic
{
    public class AdminBusinessLogic
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<CartItem> _cartItemRepository;

        private readonly IRepository<Category> _categoryRepository;

        private readonly IRepository<Photo> _photoRepository;
        private readonly IRepository<Discount> _discountRepository;
        private readonly IRepository<Cart> _cartRepository;


        // Services declaration
        ProductService _productService;
        DiscountService _discountService;
        CategoryService _categoryService;
        public AdminBusinessLogic(
            IRepository<Product> productRepository,
            IRepository<CartItem> cartItemRepository,
            IRepository<Category> categoryRepository,
            IRepository<Photo> photoRepository,
            IRepository<Discount> discountRepository,
            IRepository<Cart> cartRepository
        )
        {
            _productRepository = productRepository;
            _cartItemRepository = cartItemRepository;

            _categoryRepository = categoryRepository;

            _photoRepository = photoRepository;
            _discountRepository = discountRepository;
            _cartRepository = cartRepository;



            // Create interface for each Service
            _productService = new ProductService(_productRepository);
            _discountService = new DiscountService(_discountRepository);
            _categoryService = new CategoryService(_categoryRepository);
        }

        public async Task<IEnumerable<ProductViewModel>> GetIndexView()
        {
            var products = await _productRepository.GetAll();

            //var productsViews = _productService.GetProductViewModels(products).ToList();

            var productsViews = products.Select(item => new ProductViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                TotalQuantity = item.Quantity,
                Price = item.Price,
                CategoryNames = item.ProductCategories.Select(productCat => productCat.Category.Name).ToList(),
                DiscountViews = item.ProductDiscounts
                    .Select(productDisc => _discountService.ToDiscountView(productDisc.Discount)),
            });

            return productsViews;
        }

        public async Task<ProductCreateViewModel> GetCreateProductView()
        {
            ProductCreateViewModel productCreateViewModel = new ProductCreateViewModel();
            productCreateViewModel.CategoryResponseItems = await _categoryService.CreateDropdownCategory();
            productCreateViewModel.DiscountItems = await _discountService.CreateDropdownDiscounts();

            return productCreateViewModel;
        }

        public async Task PostCreateProductViewModel(ProductCreateViewModel productCreateViewModel)
        {
            await _productService.CreateProduct(productCreateViewModel);
        }

        public async Task<ProductCreateViewModel> GetEditProductView(int itemId)
        {
            var productCreateView = await _productService.GetProductCreateViewModelById(itemId);
            productCreateView.CategoryResponseItems = await _categoryService.CreateDropdownCategory();
            productCreateView.DiscountItems = await _discountService.CreateDropdownDiscounts();
            return productCreateView;
        }
    }
}
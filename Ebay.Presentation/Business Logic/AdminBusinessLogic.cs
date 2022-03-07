using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<ProductDiscount> _productDiscountRepository;


        // Services declaration
        private readonly ProductService _productService;
        private readonly DiscountService _discountService;
        private readonly CategoryService _categoryService;

        private readonly ProductCategoryService _productCategoryService;
        private readonly ProductDiscountService _productDiscountService;
        public AdminBusinessLogic(
            IRepository<Product> productRepository,
            IRepository<CartItem> cartItemRepository,
            IRepository<Category> categoryRepository,
            IRepository<Photo> photoRepository,
            IRepository<Discount> discountRepository,
            IRepository<Cart> cartRepository,
            IRepository<ProductCategory> productCategory,
            IRepository<ProductDiscount> productDiscount
        )
        {
            _productRepository = productRepository;
            _cartItemRepository = cartItemRepository;

            _categoryRepository = categoryRepository;

            _photoRepository = photoRepository;
            _discountRepository = discountRepository;
            _cartRepository = cartRepository;

            
            _productCategoryRepository = productCategory;
            _productDiscountRepository = productDiscount;


            // Create interface for each Service
            _productService = new ProductService(_productRepository);
            _discountService = new DiscountService(_discountRepository);
            _categoryService = new CategoryService(_categoryRepository);
            _productCategoryService = new ProductCategoryService(_productCategoryRepository);
            _productDiscountService = new ProductDiscountService(_productDiscountRepository);
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

            productCreateViewModel.Id = await _productService.GetNumberOfRecords() + 1;
            productCreateViewModel.CategoryResponseItems = await _categoryService.CreateDropdownCategory();
            productCreateViewModel.DiscountItems = await _discountService.CreateDropdownDiscounts();

            return productCreateViewModel;
        }
        
        public async Task PostCreateProductViewModel(ProductCreateViewModel productCreateViewModel)
        {
            var product = CreateProductForDb(productCreateViewModel, true);
            await _productRepository.Insert(product);
        }

        public async Task<ProductCreateViewModel> GetEditProductView(int itemId)
        {
            var productCreateView = await _productService.GetProductCreateViewModelById(itemId);
            var categoryResponseItems = await _categoryService.CreateDropdownCategory();
            var discountResponseItems = await _discountService.CreateDropdownDiscounts();

            var selectedCategoriesId = await _productService.GetSelectedCategoriesId(itemId);
            var categoryResponseSelectedItems = categoryResponseItems
                .Select(item => new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = selectedCategoriesId.Contains(int.Parse(item.Value))
                });

            var selectedDiscountsId = await _productService.GetSelectedDiscountId(itemId);
            var discountsResponseSelectedItems = discountResponseItems
                .Select(item => new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = selectedDiscountsId.Contains(int.Parse(item.Value))
                });

            productCreateView.CategoryResponseItems = categoryResponseSelectedItems.ToList();
            productCreateView.DiscountItems = discountsResponseSelectedItems.ToList();
            return productCreateView;
        }

        public async Task UpdateProduct(ProductCreateViewModel viewModel)
        {
            var product = CreateProductForDb(viewModel, false);
            await _productRepository.Update(product);
        }

        
        private Product CreateProductForDb(ProductCreateViewModel productCreateViewModel, bool ignoreId)
        {
            var categories = _categoryService.GetSelectedCategories(productCreateViewModel);
            var discounts = _discountService.GetSelectedDiscounts(productCreateViewModel);

            //await _productService.CreateProduct(productCreateViewModel, categories, discounts);
            var product = _productService.CreateProduct(productCreateViewModel, ignoreId);

            var createdProductCategory = _productCategoryService.GetProductCategories(categories, product);
            var createdProductDiscounts = _productDiscountService.GetProductDiscounts(discounts, product);
            product.ProductCategories = createdProductCategory;
            product.ProductDiscounts = createdProductDiscounts;
            return product;
        }
    }
}
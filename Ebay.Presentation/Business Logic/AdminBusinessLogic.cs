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
        /// <summary>
        ///  Method <c>GetProductsViews</c> gets all <c>ProductViewModel</c> for visualization in UI.
        /// </summary>
        /// <returns>
        ///     List of all products in the DB.
        /// </returns>
        public async Task<IEnumerable<ProductViewModel>> GetProductsViews()
        {
            var products = await _productRepository.GetAll();

            //var productsViews = _productService.GetProductViewModels(products).ToList();

            var productsViews = products.Select(item => CreateProductView(item));

            return productsViews;
        }

        public async Task<ProductViewModel> GetProductView(int id)
        {
            var product = await _productRepository.Get(id);
            var productView = CreateProductView(product);

            return productView;
        }
        /// <summary>
        ///  Method <c>GetCreateProductView</c> gets the <c>ProductCreateViewModel</c> for the visualization in UI.
        /// </summary>
        /// <returns>
        ///     <c>ProductCreateViewModel</c> entity.
        /// </returns>
        public async Task<ProductCreateViewModel> GetCreateProductView()
        {
            ProductCreateViewModel productCreateViewModel = new ProductCreateViewModel();

            productCreateViewModel.Id = await _productService.GetNumberOfRecords() + 1;
            productCreateViewModel.CategoryResponseItems = await _categoryService.CreateDropdownCategory();
            productCreateViewModel.DiscountItems = await _discountService.CreateDropdownDiscounts();

            return productCreateViewModel;
        }
        /// <summary>
        ///  Method <c>PostCreateProductViewModel</c> adds new product in the database.
        /// </summary>
        /// <param name="productCreateViewModel">
        ///     The product which will be Inserted in the Database.
        /// </param>
        public async Task PostCreateProductViewModel(ProductCreateViewModel productCreateViewModel)
        {
            var product = await CreateProductForDb(productCreateViewModel, true);
            await _productRepository.Insert(product);
        }
        /// <summary>
        ///  Method <c>GetProductView</c> gets the <c>ProductCreateViewModel</c> for visualization in UI.
        /// </summary>
        /// <param name="itemId">
        ///     Id of the product which is need to be returned.
        /// </param>
        /// <returns>
        ///     <c>ProductCreateViewModel</c> entity.
        /// </returns>
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
        /// <summary>
        /// <c>async</c> method <c>UpdateProduct</c> updates specified <c>Product</c> entity in the repository.
        /// </summary>
        /// <param name="viewModel">Model which is sent from the controller</param>
        public async Task UpdateProduct(ProductCreateViewModel viewModel)
        {
            var product = await CreateProductForDb(viewModel, false);
            await _productRepository.Update(product);
        }
        /// <summary>
        ///  Method <c>CreateProductForDb</c> transforms <c>ProductCreateView</c> in <c>Product</c>.
        /// </summary>
        /// <param name="productCreateViewModel">
        ///     Model which is sent from the controller.
        /// </param>
        /// <param name="isProductForInserting">
        ///     Tells if the product is called for the Inserting in the database (true) or for the Updating purpose (false)
        /// </param>
        /// <returns>
        ///     <c>Product</c> entity.
        /// </returns>
        private async Task<Product> CreateProductForDb(ProductCreateViewModel productCreateViewModel, bool isProductForInserting)
        {
            await _productCategoryService.DeleteAll(productCreateViewModel.Id);
            await _productDiscountService.DeleteAll(productCreateViewModel.Id);

            var categories = _categoryService.GetSelectedCategories(productCreateViewModel);
            var discounts = _discountService.GetSelectedDiscounts(productCreateViewModel);

            //await _productService.CreateProduct(productCreateViewModel, categories, discounts);
            var product = _productService.CreateProduct(productCreateViewModel, isProductForInserting);
            List<ProductCategory> createdProductCategory;
            List<ProductDiscount> createdProductDiscounts;

            createdProductCategory = _productCategoryService.CreateProductCategories(categories, product);
            createdProductDiscounts = _productDiscountService.CreateProductDiscounts(discounts, product);
            
            product.ProductCategories = createdProductCategory;
            product.ProductDiscounts = createdProductDiscounts;
            return product;
        }

        private ProductViewModel CreateProductView(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                TotalQuantity = product.Quantity,
                Price = product.Price,
                CategoryNames = product.ProductCategories.Select(productCat => productCat.Category.Name).ToList(),
                DiscountViews = product.ProductDiscounts
                    .Select(productDisc => _discountService.ToDiscountView(productDisc.Discount)),
            };
        }
    }
}
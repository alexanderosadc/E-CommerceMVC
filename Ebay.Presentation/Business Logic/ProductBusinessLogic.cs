using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Presentation.Helpers;
using Ebay.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ebay.Presentation.Business_Logic
{
    public class ProductBusinessLogic : IProductBL
    {
        private readonly IRepository<Product> _productRepository;
 //       private readonly IRepository<CartItem> _cartItemRepository;

        private readonly IRepository<Category> _categoryRepository;

        private readonly IRepository<Photo> _photoRepository;
        private readonly IRepository<Discount> _discountRepository;
//        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<ProductDiscount> _productDiscountRepository;


        // Services declaration
        private readonly ProductService _productService;
        private readonly DiscountService _discountService;
        private readonly CategoryService _categoryService;

        private readonly ProductCategoryService _productCategoryService;
        private readonly ProductDiscountService _productDiscountService;
        public ProductBusinessLogic(
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
//            _cartItemRepository = cartItemRepository;

            _categoryRepository = categoryRepository;

            _photoRepository = photoRepository;
            _discountRepository = discountRepository;
//            _cartRepository = cartRepository;

            
            _productCategoryRepository = productCategory;
            _productDiscountRepository = productDiscount;


            // Create interface for each Service
            _productService = new ProductService(_productRepository);
            _discountService = new DiscountService(_discountRepository);
            _categoryService = new CategoryService(_categoryRepository);
            _productCategoryService = new ProductCategoryService(_productCategoryRepository);
            _productDiscountService = new ProductDiscountService(_productDiscountRepository);
        }
        public async Task Delete(string id)
        {
            var product = await _productRepository.Get(int.Parse(id));
            if (product != null)
            {
                await _productRepository.Delete(product);
            }
        }
        public async Task<ProductViewDTO> GetProductView(int id)
        {
            var product = await _productRepository.Get(id);
            var productView = DTOMapper.ToProductViewDTO(product);
            var discountSum = productView.DiscountViews
                .Where(item => item.IsActive == true)
                .Sum(item => item.DiscountPercent);

            productView.FinalPrice = _productService.GetFinalPrice(product.Price, discountSum);

            return productView;
        }
        /// <summary>
        ///  Method <c>GetProductsViews</c> gets all <c>ProductViewModel</c> for visualization in UI.
        /// </summary>
        /// <returns>
        ///     List of all products in the DB.
        /// </returns>
        public async Task<IEnumerable<ProductViewDTO>> GetProductsViews()
        {
            var products = await _productRepository.GetAll();

            //var productsViews = _productService.GetProductViewModels(products).ToList();

            var productsViews = products.Select(item => DTOMapper.ToProductViewDTO(item)).ToList();
            productsViews.ForEach(item => item.FinalPrice =
                _productService.GetFinalPrice(
                    item.Price,
                    item.DiscountViews.Where(item => item.IsActive == true).Sum(item => item.DiscountPercent)
                    )
                );

            return productsViews;
        }

        
        /// <summary>
        ///  Method <c>GetCreateProductView</c> gets the <c>ProductCreateViewModel</c> for the visualization in UI.
        /// </summary>
        /// <returns>
        ///     <c>ProductCreateViewModel</c> entity.
        /// </returns>
        public async Task<ProductCreateDTO> GetProductCreateView()
        {
            ProductCreateDTO productCreateViewModel = new ProductCreateDTO();
            var productCategories = await _categoryRepository.GetAll();
            var productDiscounts = await _discountRepository.GetAll();

            productCreateViewModel.Id = await _productService.GetNumberOfRecords() + 1;
            productCreateViewModel.CategoryResponseItems = await DropdownHelper.CreateDropdownCategory(productCategories);
            productCreateViewModel.DiscountItems = await DropdownHelper.CreateDropdownDiscounts(productDiscounts);

            return productCreateViewModel;
        }
        /// <summary>
        ///  Method <c>PostCreateProductViewModel</c> adds new product in the database.
        /// </summary>
        /// <param name="productCreateViewModel">
        ///     The product which will be Inserted in the Database.
        /// </param>
        public async Task PostCreateProductDTO(ProductCreateDTO productCreateViewModel)
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
        public async Task<ProductCreateDTO> GetEditProductView(int itemId)
        {
            var productCreateView = await _productService.GetProductCreateViewModelById(itemId);
            var productCategories = await _categoryRepository.GetAll();
            var productDiscounts = await _discountRepository.GetAll();

            productCreateView.CategoryResponseItems = await DropdownHelper.CreateDropdownCategory(productCategories);
            productCreateView.DiscountItems = await DropdownHelper.CreateDropdownDiscounts(productDiscounts);

            var selectedCategoriesId = await _productService.GetSelectedCategoriesId(itemId);
            productCreateView.CategoryResponseItems
                .ForEach(item => 
                {
                    item.Selected = selectedCategoriesId.Contains(int.Parse(item.Value));
                });

            var selectedDiscountsId = await _productService.GetSelectedDiscountId(itemId);
            productCreateView.DiscountItems
                .ForEach(item => 
                {
                    item.Selected = selectedDiscountsId.Contains(int.Parse(item.Value));
                });

            return productCreateView;
        }
        /// <summary>
        /// <c>async</c> method <c>UpdateProduct</c> updates specified <c>Product</c> entity in the repository.
        /// </summary>
        /// <param name="viewModel">Model which is sent from the controller</param>
        public async Task UpdateProduct(ProductCreateDTO dto)
        {
            var product = await CreateProductForDb(dto, false);
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
        public async Task<Product> CreateProductForDb(ProductCreateDTO productCreateViewModel, bool isProductForInserting)
        {
            await _productCategoryService.DeleteAll(productCreateViewModel.Id);
            await _productDiscountService.DeleteAll(productCreateViewModel.Id);

            var categories = _categoryService.GetSelectedCategories(productCreateViewModel);
            var discounts = _discountService.GetSelectedDiscounts(productCreateViewModel);

            //await _productService.CreateProduct(productCreateViewModel, categories, discounts);
            
            var product = DTOMapper.ToProduct(productCreateViewModel, isProductForInserting);
            List<ProductCategory> createdProductCategory;
            List<ProductDiscount> createdProductDiscounts;

            createdProductCategory = _productCategoryService.CreateProductCategories(categories, product);
            createdProductDiscounts = _productDiscountService.CreateProductDiscounts(discounts, product);
            
            product.ProductCategories = createdProductCategory;
            product.ProductDiscounts = createdProductDiscounts;
            return product;
        }
    }
}
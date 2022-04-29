using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Infrastructure.Helpers;
using Ebay.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ebay.Infrastructure.Interfaces.AdminPresentation.Services;
using Ebay.Infrastructure.Interfaces.AdminPresentation;

namespace Ebay.Infrastructure.Business_Logic
{
    public class ProductBusinessLogic : IProductBL
    {
        // Constants
        private const int PageSize = 10;

        // Repositories 
        private readonly IRepository<Product> _productRepository;

        private readonly IRepository<Category> _categoryRepository;

        private readonly IRepository<Photo> _photoRepository;
        private readonly IRepository<Discount> _discountRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<ProductDiscount> _productDiscountRepository;


        // Services declaration
        private readonly IDiscountService _discountService;
        private readonly ICategoryService _categoryService;

        private readonly ProductCategoryService _productCategoryService;
        private readonly ProductDiscountService _productDiscountService;
        public ProductBusinessLogic(
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IRepository<Photo> photoRepository,
            IRepository<Discount> discountRepository,
            IRepository<ProductCategory> productCategory,
            IRepository<ProductDiscount> productDiscount,
            ICategoryService categoryService,
            IDiscountService discountService
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
            _discountService = discountService;
            _categoryService = categoryService;
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
            var discountSum = productView.Discounts
                .Where(item => item.IsActive == true)
                .Sum(item => item.DiscountPercent);

            productView.FinalPrice = PriceCalulatorHelper.GetFinalPrice(product.Price, discountSum);

            return productView;
        }
        /// <summary>
        ///  Method <c>GetProductsViews</c> gets all <c>ProductViewModel</c> for visualization in UI.
        /// </summary>
        /// <returns>
        ///     List of all products in the DB.
        /// </returns>
        public async Task<ProductViewListDTO> GetProductsViews(int currentPageNumber, int pageSize = 10)
        {
            var products =  _productRepository.GetFirstValues(currentPageNumber, pageSize);

            //var productsViews = _productService.GetProductViewModels(products).ToList();

            var productsViews = products.Select(item => DTOMapper.ToProductViewDTO(item)).ToList();
            productsViews.ForEach(item => item.FinalPrice =
                PriceCalulatorHelper.GetFinalPrice(
                    item.Price,
                    item.Discounts.Where(item => item.IsActive == true).Sum(item => item.DiscountPercent)
                    )
                );

            return new ProductViewListDTO
            {
                Products = productsViews,
                PaginationInfo = new PagingInfo
                {
                    TotalItems = await _productRepository.GetNumberOfItems(),
                    ItemsPerPage = pageSize,
                    CurrentPage = currentPageNumber,
                }
            };
        }

        public async Task<ProductViewListDTO> GetProductsViewsByCategoryDiscounts(
            int currentPageNumber,
            int? categoryId,
            int? discountId,
            int pageSize = PageSize
            )
        {
            
            var products = await _productRepository.GetAll();
            var productsCombined = GetCombinedListOfProducts(categoryId, discountId, products.ToList());
            var productsViews = new List<ProductViewDTO>();
            // Products which exist in the list for 2 times are considered items which are both in discount and category

            if (productsCombined.Count > 0)
            {
                productsViews = GetProductsWithFinalPrice(productsCombined);
            }

            return new ProductViewListDTO
                {
                    Products = productsViews,
                    PaginationInfo = new PagingInfo
                    {
                        TotalItems = productsViews.Count(),
                        ItemsPerPage = pageSize,
                        CurrentPage = currentPageNumber,
                    }
                };
            
        }

        public List<Product> GetProductsByCategoryId(int categoryId, List<Product> products)
        {
            return products.Where(item =>
            {
                var ids = item.ProductCategories.Select(category => category.Id);
                if (ids.Contains(categoryId))
                    return true;
                return false;
            }).ToList();
        }

        public List<Product> GetProductsByDiscountId(int discountId, List<Product> products)
        {
            return products.Where(item =>
            {
                var ids = item.ProductDiscounts.Select(discount => discount.Id);
                if (ids.Contains(discountId))
                    return true;
                return false;
            }).ToList();
        }

        public List<Product> GetCombinedListOfProducts(int? categoryId, int? discountId, List<Product> products)
        {
            List<Product> productsCombined = new List<Product>();

            if (categoryId != null)
            {
                var productsWithCategoryId = GetProductsByCategoryId(categoryId.Value, products.ToList());
                productsCombined.AddRange(productsWithCategoryId);
            }

            if (discountId != null)
            {
                var productsWithDiscountId = GetProductsByDiscountId(discountId.Value, products.ToList());
                productsCombined.AddRange(productsWithDiscountId);

                if (categoryId != null)
                {
                    productsCombined = productsCombined
                    .GroupBy(item => item)
                    .Where(product => product.Count() > 1)
                    .Select(y => y.Key)
                    .ToList();
                }
            }

            return productsCombined;
        }

        public List<ProductViewDTO> GetProductsWithFinalPrice(List<Product> products)
        {
            var productsViews = products.Select(item => DTOMapper.ToProductViewDTO(item)).ToList();
            productsViews.ForEach(item => item.FinalPrice =
                PriceCalulatorHelper.GetFinalPrice(
                    item.Price,
                    item.Discounts.Where(item => item.IsActive == true).Sum(item => item.DiscountPercent)
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

            var lastInsertedProduct = await _productRepository.GetLastItem();
            
            if(lastInsertedProduct != null)
            {
                productCreateViewModel.Id = lastInsertedProduct.Id + 1;
            }
            else
            {
                productCreateViewModel.Id = 1;
            }
            
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
            //var productCreateView = await _productService.GetProductCreateViewModelById(itemId);
            var product = await _productRepository.Get(itemId);
            var productCreateView = DTOMapper.ToProductCreateDTO(product);
            var productCategories = await _categoryRepository.GetAll();
            var productDiscounts = await _discountRepository.GetAll();

            productCreateView.CategoryResponseItems = await DropdownHelper.CreateDropdownCategory(productCategories);
            productCreateView.DiscountItems = await DropdownHelper.CreateDropdownDiscounts(productDiscounts);

            var selectedCategoriesId = product.ProductCategories.Select(item => item.CategoryId).ToList();
            //var selectedCategoriesId = await _productService.GetSelectedCategoriesId(itemId);
            productCreateView.CategoryResponseItems
                .ForEach(item => 
                {
                    item.Selected = selectedCategoriesId.Contains(int.Parse(item.Value));
                });

            var selectedDiscountsId = product.ProductDiscounts.Select(item => item.DiscountId).ToList();
            //var selectedDiscountsId = await _productService.GetSelectedDiscountId(itemId);
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

            var photos = productCreateViewModel.Files
                .Select(item => 
                    new Photo
                    {
                        Name = item.FileName,
                        BinaryData = FileHelper.TransformToBinary(item)
                    });

            product.Photos = photos.ToList();

            return product;
        }


        public async Task DeletePhoto(string id)
        {
            var photo = await _photoRepository.Get(int.Parse(id));
            if (photo == null)
                throw new ArgumentException("Product with this id does not exist");
            await _photoRepository.Delete(photo);
        }


        public IEnumerable<ProductViewDTO> SortProductViews(string sortCondition, IEnumerable<ProductViewDTO> products)
        {
            IEnumerable<ProductViewDTO> filteredProducts;
            if(sortCondition == "name")
            {
                filteredProducts = products.OrderBy(item => item.Name);
            }
            else if(sortCondition == "priceAsc")
            {
                filteredProducts = products.OrderBy(item => item.Price);
            }
            else if(sortCondition == "priceDesc")
            {
                filteredProducts = products.OrderByDescending(item => item.Price);
            }
            else
            {
                filteredProducts = products;
            }

            return filteredProducts;
        }

        public IEnumerable<ProductViewDTO> SearchProductByName(string name, IEnumerable<ProductViewDTO> products)
        {
            return products.Where(item => item.Name.ToLower().Contains(name.ToLower()));
        }
    }
}
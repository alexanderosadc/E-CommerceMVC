using Ebay.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Ebay.Presentation.Services;
using Ebay.Infrastructure.ViewModels.Admin.Index;

namespace Ebay.Presentation.Controllers
{
    public class AdminController : Controller
    {
        // Repository Declaration
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<CartItem> _cartItemRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Photo> _photoRepository;
        private readonly IRepository<Discount> _discountRepository;
        private readonly IRepository<Cart> _CartRepository;

        // Services declaration
        ProductService _productService;

        public AdminController(
            IRepository<Product> productRepository,
            IRepository<CartItem> cartItemRepository,
            IRepository<Category> categoryRepository,
            IRepository<Photo> photoRepository,
            IRepository<Discount> discountRepository,
            IRepository<Cart> CartRepository
            )
        {
            _productRepository = productRepository;
            _cartItemRepository = cartItemRepository;
            _categoryRepository = categoryRepository;
            _photoRepository = photoRepository;
            _discountRepository = discountRepository;
            _CartRepository = CartRepository;

            _productService = new ProductService(_productRepository, _discountRepository, _categoryRepository);
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductViewModel> products = await _productService.GetAllProducts();
            return View(products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            var viewProduct = new ProductCreateViewModel();
            await _productService.CreateDropdownSelectedItems(viewProduct);
            return View(viewProduct);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateViewModel modelView)
        {
            if(ModelState.IsValid)
            {
                await _productService.CreateProduct(modelView);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(CreateProduct));
        }
    }
}

using Ebay.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ebay.Infrastructure.Repository;
using Ebay.Domain.Entities;

namespace Ebay.Presentation.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<CartItem> _cartItemRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<CategoryField> _categoryFieldRepository;
        private readonly IRepository<Discount> _discountRepository;
        private readonly IRepository<UserCart> _userCartRepository;
        public AdminController(
            IRepository<Product> productRepository,
            IRepository<CartItem> cartItemRepository,
            IRepository<Category> categoryRepository,
            IRepository<CategoryField> categoryFieldRepository,
            IRepository<Discount> discountRepository,
            IRepository<UserCart> userCartRepository
            )
        {
            _productRepository = productRepository;
            _cartItemRepository = cartItemRepository;
            _categoryRepository = categoryRepository;
            _categoryFieldRepository = categoryFieldRepository;
            _discountRepository = discountRepository;
            _userCartRepository = userCartRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

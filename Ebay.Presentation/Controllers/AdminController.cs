﻿using Ebay.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Presentation.Business_Logic;

namespace Ebay.Presentation.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminBusinessLogic _adminBusinessLogic;
        // Services declaration

        public AdminController(
            IRepository<Product> productRepository,
            IRepository<CartItem> cartItemRepository,
            IRepository<Category> categoryRepository,
            IRepository<Photo> photoRepository,
            IRepository<Discount> discountRepository,
            IRepository<Cart> cartRepository,
            IRepository<ProductCategory> productCategoryRepository,
            IRepository<ProductDiscount> productDiscountRepository
            )
        {
            _adminBusinessLogic = new AdminBusinessLogic(
                productRepository,
                cartItemRepository,
                categoryRepository,
                photoRepository,
                discountRepository,
                cartRepository,
                productCategoryRepository,
                productDiscountRepository
                );
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductViewModel> products = await _adminBusinessLogic.GetProductsViews();
            return View(products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            var product = await _adminBusinessLogic.GetCreateProductView();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateViewModel modelView)
        {
            if(ModelState.IsValid)
            {
                await _adminBusinessLogic.PostCreateProductViewModel(modelView);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(CreateProduct));
        }

        public async Task<IActionResult> EditProduct(int itemId)
        {
            var productCreateView = await _adminBusinessLogic.GetEditProductView(itemId);
            return View(productCreateView);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductCreateViewModel modelView)
        {
            if (ModelState.IsValid)
            {
                await _adminBusinessLogic.UpdateProduct(modelView);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(EditProduct), new {itemId = modelView.Id});
        }

        public async Task<IActionResult> ProductDetails(int itemId)
        {
            var productView = await _adminBusinessLogic.GetProductView(itemId);
            return View(productView);
        }
    }
}
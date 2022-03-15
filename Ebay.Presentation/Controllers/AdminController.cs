﻿using Ebay.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Presentation.Business_Logic;
using Microsoft.AspNetCore.Authorization;
using Ebay.Infrastructure.ViewModels.Admin;
using Ebay.Infrastructure.ViewModels.Admin.CreateCategory;

namespace Ebay.Presentation.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        // Business Logic 
        private readonly ProductBusinessLogic _productBusinessLogic;
        private readonly CategoryBusinessLogic _categoryBusinessLogic;
        private readonly DiscountBusinessLogic _discountBusinessLogic;
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
            _productBusinessLogic = new ProductBusinessLogic(
                productRepository,
                cartItemRepository,
                categoryRepository,
                photoRepository,
                discountRepository,
                cartRepository,
                productCategoryRepository,
                productDiscountRepository
                );
            _categoryBusinessLogic = new CategoryBusinessLogic(categoryRepository);
            _discountBusinessLogic = new DiscountBusinessLogic(discountRepository);
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductViewDTO> products = await _productBusinessLogic.GetProductsViews();
            return View(products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            var product = await _productBusinessLogic.GetCreateProductView();
            ViewBag.SelectedDiscounts = product.DiscountItems;
            ViewBag.SelectedCategories = product.CategoryResponseItems;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDTO modelView)
        {
            if(ModelState.IsValid)
            {
                await _productBusinessLogic.PostCreateProductViewModel(modelView);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(CreateProduct));
        }

        public async Task<IActionResult> EditProduct(int itemId)
        {
            var productCreateView = await _productBusinessLogic.GetEditProductView(itemId);
            return View(productCreateView);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductCreateDTO modelView)
        {
            if (ModelState.IsValid)
            {
                await _productBusinessLogic.UpdateProduct(modelView);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(EditProduct), new {itemId = modelView.Id});
        }

        public async Task<IActionResult> ProductDetails(int itemId)
        {
            var productView = await _productBusinessLogic.GetProductView(itemId);
            return View(productView);
        }

        public async Task<IActionResult> DeleteProduct(int itemId)
        {
            await _productBusinessLogic.DeleteProduct(itemId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ShowCategories()
        {
            IEnumerable<CategoryViewDTO> categories = await _categoryBusinessLogic.GetCategoyDTO();
            return View(categories);
        }

        public async Task<IActionResult> CreateCategory()
        {
            CategoryCreateDTO category = await _categoryBusinessLogic.GetCategoryCreateDTO();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateDTO categoryViewModel)
        {
            if(ModelState.IsValid)
            {
                await _categoryBusinessLogic.CreateNewCategory(categoryViewModel);
                return RedirectToAction(nameof(ShowCategories));
            }
            
            return View(nameof(CreateCategory));
        }
        public async Task<IActionResult> EditCategory(int itemId)
        {
            var categoryCreateViewModel = await _categoryBusinessLogic.EditCategory(itemId);

            return View(categoryCreateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryCreateDTO categoryCreateViewModel)
        {
            if(ModelState.IsValid)
            {
                await _categoryBusinessLogic.UpdateCategory(categoryCreateViewModel);
                return RedirectToAction(nameof(ShowCategories));
            }

            return RedirectToAction(nameof(EditCategory), new {itemId = categoryCreateViewModel .Id});
        }

        public async Task<IActionResult> DeleteCategory(int itemId)
        {
            await _categoryBusinessLogic.DeleteCategory(itemId);
            return RedirectToAction(nameof(ShowCategories));
        }


        public async Task<IActionResult> ShowDiscounts()
        {
            IEnumerable<DiscountViewDTO> discounts = await _discountBusinessLogic.GetDiscountsDTO();
            return View(discounts);
        }

        public async Task<IActionResult> CreateDiscount()
        {
            DiscountViewDTO discount = await _discountBusinessLogic.GetDiscountDTO();
            return View(discount);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount(DiscountViewDTO discountViewModel)
        {
            if (ModelState.IsValid)
            {
                await _discountBusinessLogic.CreateNewDiscount(discountViewModel);
                return RedirectToAction(nameof(ShowDiscounts));
            }

            return View(nameof(CreateDiscount));
        }
        public async Task<IActionResult> EditDiscount(int itemId)
        {
            var discountViewModel = await _discountBusinessLogic.EditDiscount(itemId);

            return View(discountViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDiscount(DiscountViewDTO discountViewModel)
        {
            if (ModelState.IsValid)
            {
                await _discountBusinessLogic.UpdateDiscount(discountViewModel);
                return RedirectToAction(nameof(ShowDiscounts));
            }

            return RedirectToAction(nameof(EditDiscount), new { itemId = discountViewModel.Id });
        }

        public async Task<IActionResult> DeleteDiscount(int itemId)
        {
            await _discountBusinessLogic.DeleteDiscount(itemId);
            return RedirectToAction(nameof(ShowDiscounts));
        }
    }
}
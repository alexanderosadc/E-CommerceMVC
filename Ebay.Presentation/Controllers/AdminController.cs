using Ebay.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Presentation.Business_Logic;
using Microsoft.AspNetCore.Authorization;
using Ebay.Infrastructure.ViewModels.Admin;
using Ebay.Infrastructure.ViewModels.Admin.CreateCategory;
using Ebay.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Ebay.Infrastructure.ViewModels.Admin.Users;

namespace Ebay.Presentation.Controllers
{
    public class AdminController : Controller
    {
        // Business Logic 
        private readonly IProductBL _productBusinessLogic;
        private readonly ICategoryBL _categoryBusinessLogic;
        private readonly IDiscountBL _discountBusinessLogic;
        private readonly IUserBL _userBusinessLogic;
        
        // Services declaration
        public AdminController(
            IProductBL productBusinessLogic,
            ICategoryBL categoryBusinessLogic,
            IDiscountBL discountBusinessLogic,
            IUserBL userBusinessLogic
            )
        {
            _productBusinessLogic = productBusinessLogic;
            _categoryBusinessLogic = categoryBusinessLogic;
            _discountBusinessLogic = discountBusinessLogic;
            _userBusinessLogic = userBusinessLogic;
        }

        [Authorize(Roles = "moderator,admin")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductViewDTO> products = await _productBusinessLogic.GetProductsViews();
            return View(products);
        }
        [Authorize(Roles = "moderator,admin")]
        public async Task<IActionResult> CreateProduct()
        {
            var product = await _productBusinessLogic.GetProductCreateView();
            ViewBag.SelectedDiscounts = product.DiscountItems;
            ViewBag.SelectedCategories = product.CategoryResponseItems;
            return View(product);
        }

        [Authorize(Roles = "moderator,admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDTO modelView)
        {
            if(ModelState.IsValid)
            {
                await _productBusinessLogic.PostCreateProductDTO(modelView);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(CreateProduct));
        }

        [Authorize(Roles = "moderator,admin")]
        public async Task<IActionResult> EditProduct(int itemId)
        {
            var productCreateView = await _productBusinessLogic.GetEditProductView(itemId);
            return View(productCreateView);
        }

        [Authorize(Roles = "moderator,admin")]
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

        [Authorize(Roles = "moderator,admin")]
        public async Task<IActionResult> ProductDetails(int itemId)
        {
            var productView = await _productBusinessLogic.GetProductView(itemId);
            return View(productView);
        }

        [Authorize(Roles = "moderator,admin")]
        public async Task<IActionResult> DeleteProduct(string itemId)
        {
            await _productBusinessLogic.Delete(itemId);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ShowCategories()
        {
            IEnumerable<CategoryViewDTO> categories = await _categoryBusinessLogic.GetCategoyDTO();
            return View(categories);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCategory()
        {
            CategoryCreateDTO category = await _categoryBusinessLogic.GetCategoryCreateDTO();
            return View(category);
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditCategory(int itemId)
        {
            var categoryCreateViewModel = await _categoryBusinessLogic.EditCategory(itemId);

            return View(categoryCreateViewModel);
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCategory(string itemId)
        {
            await _categoryBusinessLogic.Delete(itemId);
            return RedirectToAction(nameof(ShowCategories));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ShowDiscounts()
        {
            IEnumerable<DiscountViewDTO> discounts = await _discountBusinessLogic.GetDiscountsDTO();
            return View(discounts);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateDiscount()
        {
            DiscountViewDTO discount = await _discountBusinessLogic.GetDiscountDTO();
            return View(discount);
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteDiscount(string itemId)
        {
            await _discountBusinessLogic.Delete(itemId);
            return RedirectToAction(nameof(ShowDiscounts));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ShowUsers()
        { 
            var users = await _userBusinessLogic.GetUsers();
            return View(users);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateUser()
        {
            AppUserCreateDTO appUserCreate = new AppUserCreateDTO();
            return View(appUserCreate);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(AppUserCreateDTO dto)
        {
            if(ModelState.IsValid)
            {
                await _userBusinessLogic.CreateUser(dto);
                return RedirectToAction(nameof(ShowUsers));
            }
            
            return View(nameof(CreateUser));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditUser(string itemId)
        {
            AppUserCreateDTO appUserCreate = await _userBusinessLogic.GetUserDTO(itemId);
            return View(appUserCreate);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> EditUser(bool isModerator, string userId)
        {
            if(ModelState.IsValid)
            {
                await _userBusinessLogic.EditUser(isModerator, userId);
            }
            return RedirectToAction(nameof(ShowUsers));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string itemId)
        {
            await _userBusinessLogic.Delete(itemId);
            return RedirectToAction(nameof(ShowUsers));
        }
    }
}
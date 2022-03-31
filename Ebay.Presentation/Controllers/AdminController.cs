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
        private readonly IValidationBL _validationBusinessLogic;
        
        // Services declaration
        public AdminController(
            IProductBL productBusinessLogic,
            ICategoryBL categoryBusinessLogic,
            IDiscountBL discountBusinessLogic,
            IUserBL userBusinessLogic,
            IValidationBL validationBusinessLogic
            )
        {
            _productBusinessLogic = productBusinessLogic;
            _categoryBusinessLogic = categoryBusinessLogic;
            _discountBusinessLogic = discountBusinessLogic;
            _userBusinessLogic = userBusinessLogic;
            _validationBusinessLogic = validationBusinessLogic;
        }

        [Authorize(Roles = "moderator,admin")]
        public async Task<IActionResult> Index(int currentPageNumber = 1)
        {
            /*IEnumerable<ProductViewDTO> products = await _productBusinessLogic
                .GetProductsViews(currentPageNumber);*/
            ProductViewListDTO productListDTO = await _productBusinessLogic
                .GetProductsViews(currentPageNumber);
            
            return View(productListDTO);
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
        public async Task<IActionResult> CreateProduct(ProductCreateDTO dto)
        {
            var productCreate = await _productBusinessLogic.GetProductCreateView();
            dto.CategoryResponseItems = productCreate.CategoryResponseItems;
            dto.DiscountItems = productCreate.DiscountItems;

            if (ModelState.IsValid)
            {
                
                var validationErrors = await _validationBusinessLogic.ValidateProduct(dto.Name);
                if (validationErrors.Count > 0)
                {
                    
                    foreach (var error in validationErrors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                    return View(nameof(CreateProduct), dto);
                }
                await _productBusinessLogic.PostCreateProductDTO(dto);
                return RedirectToAction(nameof(Index), dto);
            }
            return View(nameof(CreateProduct), dto);
        }

        [Authorize(Roles = "moderator,admin")]
        public async Task<IActionResult> EditProduct(int itemId)
        {
            var productCreateView = await _productBusinessLogic.GetEditProductView(itemId);
            return View(productCreateView);
        }

        [Authorize(Roles = "moderator,admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _productBusinessLogic.UpdateProduct(dto);
                return RedirectToAction(nameof(Index));
            }
            //return RedirectToAction(nameof(EditProduct), new {itemId = modelView.Id});
            var dropdownCategories = await _categoryBusinessLogic.GetDropdownCategories();
            var dropdownDiscounts = await _discountBusinessLogic.GetDropdownDiscounts();

            dto.CategoryResponseItems = dropdownCategories;
            dto.DiscountItems = dropdownDiscounts;
            return View(nameof(EditProduct), dto);
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
        public async Task<IActionResult> CreateCategory(CategoryCreateDTO dto)
        {
            var categoryCreate = await _categoryBusinessLogic.GetCategoryCreateDTO();
            dto.AllChildrenCategories = categoryCreate.AllChildrenCategories;
            if (ModelState.IsValid)
            {
                var validationErrors = await _validationBusinessLogic.ValidateCategory(dto.Name);
                if (validationErrors.Count > 0)
                {

                    foreach (var error in validationErrors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                    return View(nameof(CreateCategory), dto);
                }
                await _categoryBusinessLogic.CreateNewCategory(dto);
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

            return RedirectToAction(nameof(EditCategory), new {itemId = categoryCreateViewModel.Id});
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
        public async Task<IActionResult> CreateDiscount(DiscountViewDTO dto)
        {
            if (ModelState.IsValid)
            {
                var validationErrors = await _validationBusinessLogic.ValidateDiscount(dto.Name);
                if (validationErrors.Count > 0)
                {
                    foreach (var error in validationErrors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                    return View(nameof(CreateDiscount), dto);
                }
                await _discountBusinessLogic.CreateNewDiscount(dto);
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
                var validationErrors = await _validationBusinessLogic.ValidateUser(dto.UserName, dto.Email);
                if(validationErrors.Count > 0)
                {
                    foreach(var error in validationErrors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                    return View(nameof(CreateUser), dto);
                }
                await _userBusinessLogic.CreateUser(dto);
                return RedirectToAction(nameof(ShowUsers));
            }
            
            return View(nameof(CreateUser), dto);
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

        [Authorize(Roles = "moderator,admin")]
        public async Task<IActionResult> DeletePhoto(string itemId, string modelId)
        {
            await _productBusinessLogic.DeletePhoto(itemId);
            var productCreateView = await _productBusinessLogic
                                            .GetEditProductView(int.Parse(modelId));
            return View(nameof(EditProduct), productCreateView);
        }
    }
}
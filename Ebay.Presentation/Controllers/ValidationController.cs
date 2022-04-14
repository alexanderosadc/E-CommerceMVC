using Ebay.Infrastructure.Interfaces;
using Ebay.Infrastructure.Interfaces.AdminPresentation;
using Ebay.Infrastructure.ViewModels.Admin.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ebay.Presentation.Controllers
{
    public class ValidationController : Controller
    {
        // Business Logic 
        private readonly IProductBL _productBusinessLogic;
        private readonly ICategoryBL _categoryBusinessLogic;
        private readonly IDiscountBL _discountBusinessLogic;
        private readonly IUserBL _userBusinessLogic;
        
        public ValidationController(
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

        /*[HttpGet]
        public async Task<IActionResult> IsUsernameAvialiable(string UserName)
        {
            bool isUserExist = await _userBusinessLogic.IsUserExist(UserName);
            return Json(isUserExist);
        }*/
    }
}

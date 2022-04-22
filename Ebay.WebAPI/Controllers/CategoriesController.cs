using Ebay.Infrastructure.Interfaces.AdminPresentation;
using Ebay.Infrastructure.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Ebay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        // Business Logic 
        private readonly IProductBL _productBusinessLogic;
        private readonly ICategoryBL _categoryBusinessLogic;
        private readonly IDiscountBL _discountBusinessLogic;
        private readonly IUserBL _userBusinessLogic;
        private readonly IValidationBL _validationBusinessLogic;

        // Services declaration
        public CategoriesController(
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CategoryViewDTO> categories = await _categoryBusinessLogic.GetCategoyDTO();
            if(categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1)
                return BadRequest();
            var categoryView = await _categoryBusinessLogic.GetCategoryDTO(id);
            return Ok(categoryView);
        }
    }
}

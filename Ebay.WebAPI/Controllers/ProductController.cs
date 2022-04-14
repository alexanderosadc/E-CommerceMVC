using Ebay.Infrastructure.Interfaces.AdminPresentation;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Microsoft.AspNetCore.Mvc;

namespace Ebay.WebAPI.Controllers
{
    [Route("api[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        // Business Logic 
        private readonly IProductBL _productBusinessLogic;
        private readonly ICategoryBL _categoryBusinessLogic;
        private readonly IDiscountBL _discountBusinessLogic;
        private readonly IUserBL _userBusinessLogic;
        private readonly IValidationBL _validationBusinessLogic;

        // Services declaration
        public ProductController(
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
        public async Task<IActionResult> GetAll([FromQuery] int currentPageNumber = 1)
        {
            if (currentPageNumber < 1)
                return BadRequest();

            ProductViewListDTO productListDTO = await _productBusinessLogic
                .GetProductsViews(currentPageNumber);

            if (productListDTO == null)
                return NotFound();

            return Ok(productListDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1)
                return BadRequest();

            var productView = await _productBusinessLogic.GetProductView(id);
            
            if (productView == null)
                return NotFound();

            return Ok(productView);
        }
    }
}

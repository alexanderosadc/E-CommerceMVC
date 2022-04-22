using Ebay.Infrastructure.Interfaces.AdminPresentation;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Microsoft.AspNetCore.Mvc;

namespace Ebay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : Controller
    {
        // Business Logic 
        private readonly IProductBL _productBusinessLogic;
        private readonly ICategoryBL _categoryBusinessLogic;
        private readonly IDiscountBL _discountBusinessLogic;
        private readonly IUserBL _userBusinessLogic;
        private readonly IValidationBL _validationBusinessLogic;

        // Services declaration
        public DiscountsController(
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
            IEnumerable<DiscountViewDTO> discounts = await _discountBusinessLogic.GetDiscountsDTO();
            if (discounts == null)
            {
                return NotFound();
            }

            return Ok(discounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1)
                return BadRequest();

            var discount = await _discountBusinessLogic.GetDiscountDTO(id);
            if(discount == null)
                return NotFound();

            return Ok(discount);
        }
    }
}

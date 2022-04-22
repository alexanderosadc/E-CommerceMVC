using Ebay.Infrastructure.Helpers;
using Ebay.Infrastructure.Interfaces.AdminPresentation;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Microsoft.AspNetCore.Mvc;

namespace Ebay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        // Business Logic 
        private readonly IProductBL _productBusinessLogic;
        

        // Services declaration
        public ProductsController(
            IProductBL productBusinessLogic
            )
        {
            _productBusinessLogic = productBusinessLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? categoryId,
            [FromQuery] int? discountId,
            [FromQuery] int pageSize = 4, 
            [FromQuery] int currentPageNumber = 1)
        {

            if (currentPageNumber < 1)
                return BadRequest();

            ProductViewListDTO productListDTO = null;

            if (categoryId != null || discountId != null)
            {
                productListDTO = await _productBusinessLogic
                .GetProductsViewsByCategoryDiscounts(currentPageNumber, categoryId, discountId, pageSize);
            }
            else if(categoryId == null && discountId == null)
            {
                productListDTO = await _productBusinessLogic
                .GetProductsViews(currentPageNumber, pageSize);
            }

            //productListDTO.Products.Select(item => item.Photos).ToList().ForEach() .Photos.ForEach(item => item.BinaryData = FileHelper.Compress(item.BinaryData));
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

           // productView.Photos.ForEach(item => item.BinaryData = FileHelper.Compress(item.BinaryData));
            if (productView == null)
                return NotFound(productView);

            return Ok(productView);
        }
    }
}

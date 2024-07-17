using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Mvc;
using AuthAPI.models;
using AuthAPI.Data;
using Swashbuckle.AspNetCore.Annotations;
using AuthAPI.models.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/products")]
    [SwaggerTag("Endpoints untuk mengelola data produk")]
    public class ProductsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;

        public ProductsController(MyDbContext context, ILogger<ProductsController> logger, IProductService productService)
        {
            _context = context;
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _productService.GetAllProducts();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountAllProducts()
        {
            var response = await _productService.CountAllProducts();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse>> GetProductById([FromRoute] int id)
        {
            var response = await _productService.GetProductById(id);
            return StatusCode(response.StatusCode, response);

        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse>> AddProduct(ProductReqDto dto)
        {
            var response = await _productService.AddProduct(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("multi")]
        public async Task<ActionResult<GenericResponse>> AddMultiProduct(List<ProductReqDto> dto)
        {
            var response = await _productService.AddMultiProduct(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductReqDto product)
        {
            var result = await _productService.UpdateProductAsync(product);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("browse")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(
            [FromQuery] string productName = "",
            [FromQuery] string units = "",
            [FromQuery] decimal? price = null)
        {
            var result = await _productService.BrowseProduct(productName, units, price);
            return StatusCode(result.StatusCode, result);
        }


    }
}

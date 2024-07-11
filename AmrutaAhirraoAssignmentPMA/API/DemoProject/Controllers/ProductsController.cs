using DemoProject.Interface;
using DemoProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Cors;

namespace DemoProject.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productsService;

        public ProductsController(IProductServices productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productsService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productsService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Products product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _productsService.AddProduct(product);
            return Ok(product);
        }

        [HttpPut]
        public async Task<IActionResult> Edit( int id,[FromBody] Products product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productsService.UpdateProduct(id,product);
            if (!result)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productsService.DeleteProduct(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

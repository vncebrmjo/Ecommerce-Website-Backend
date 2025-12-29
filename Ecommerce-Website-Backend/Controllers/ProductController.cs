using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website_Backend.Controllers
{
    [Route("api/products")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]  // GET: api/products
        public IActionResult GetProducts()
        {
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var apiKey = _configuration["ApiKeys:ExternalApi"];

            // Return actual product data
            return Ok(new[] {
            new { Id = 1, Name = "Testing" },
            new { Id = 2, Name = "Product 2" }
        });
        }

        [HttpGet("{id}")]  // GET: api/products/5
        public IActionResult GetProduct(int id)
        {
            // Return single product
            return Ok(new { Id = id, Name = $"Product {id}" });
        }
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}

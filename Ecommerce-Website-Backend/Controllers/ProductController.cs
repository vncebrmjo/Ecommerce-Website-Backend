using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // GET: api/products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100.99m },
                new Product { Id = 2, Name = "Product 2", Price = 49.99m }
            };

            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = new Product { Id = id, Name = $"Product {id}", Price = 29.99m };
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public ActionResult<Product> CreateProduct([FromBody] Product product)
        {
            // Add creation logic here
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            // Add update logic here
            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            // Add deletion logic here
            return NoContent();
        }
    }

    // Model class
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}

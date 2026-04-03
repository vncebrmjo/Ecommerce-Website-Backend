using Ecommerce_Website_Backend.Models.Request;
using Ecommerce_Website_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Website_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoryController(ProductCategoryService service) : ControllerBase
    {
        // GET api/productcategory
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await service.GetAllAsync();
            return Ok(categories);
        }

        // GET api/productcategory/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await service.GetByIdAsync(id);
            return Ok(category);
        }

        // POST api/productcategory
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCategoryRequest request)
        {
            var category = await service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        // PUT api/productcategory/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCategoryRequest request)
        {
            var category = await service.UpdateAsync(id, request);
            return Ok(category);
        }

        // DELETE api/productcategory/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
    }
}

using Ecommerce_Website_Backend.Data.Entities;

namespace Ecommerce_Website_Backend.Models.Response
{
    public class ProductCategoryResponse
    {
        public int Id { get; set; }
        public string ProductCategoryName { get; set; } = string.Empty;
        public string ProductCategoryDescription { get; set; } = string.Empty;

        // Maps entity to response — keeps raw entity out of the API
        public static ProductCategoryResponse FromEntity(ProductCategoryEntity entity) => new()
        {
            Id = entity.Id,
            ProductCategoryName = entity.ProductCategoryName,
            ProductCategoryDescription = entity.ProductCategoryDescription
        };
    }
}

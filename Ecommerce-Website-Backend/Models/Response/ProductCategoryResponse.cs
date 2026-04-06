using Ecommerce_Website_Backend.Data.Entities;

namespace Ecommerce_Website_Backend.Models.Response
{
    public record ProductCategoryResponse(
    int Id,
    string ProductCategoryName,
    string ProductCategoryDescription)
    
    {
        // Maps Entity to response - keeps raw entity out of the API
        public static ProductCategoryResponse FromEntity(ProductCategoryEntity entity) =>
            new(entity.Id, entity.ProductCategoryName, entity.ProductCategoryDescription);
    }
}

using Ecommerce_Website_Backend.Data.Entities;

namespace Ecommerce_Website_Backend.Models.Response
{
    public record ProductResponse(
        int Id,
        string ProductName,
        string ProductDescription,
        string Sku,
        decimal Price,
        int StockQuantity,
        bool IsActive,
        int ProductCategoryId,
        string ProductCategoryName,
        int? MerchantId,
        string? MerchantName)
    {
        // Maps Entity to response - keeps raw entity out of the API
        public static ProductResponse FromEntity(ProductEntity entity) =>
            new(
                Id: entity.Id,
                ProductName: entity.ProductName,
                ProductDescription: entity.ProductDescription,
                Sku: entity.Sku,
                Price: entity.Price,
                StockQuantity: entity.StockQuantity,
                IsActive: entity.IsActive,
                ProductCategoryId: entity.ProductCategoryId,
                ProductCategoryName: entity.ProductCategory.ProductCategoryName,
                MerchantId: entity.MerchantId,
                MerchantName: entity.Merchant is null ? null : $"{entity.Merchant.FirstName} {entity.Merchant.LastName}");
    }
}
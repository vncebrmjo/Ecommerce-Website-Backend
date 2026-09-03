namespace Ecommerce_Website_Backend.Data.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;

        public int ProductCategoryId { get; set; }
        public ProductCategoryEntity ProductCategory { get; set; } = null!;

        // Who owns this product. Null = owned by the platform (Admin/SuperAdmin created it).
        // Set = owned by that Merchant.
        public int? MerchantId { get; set; }
        public UserEntity? Merchant { get; set; }

    }
}

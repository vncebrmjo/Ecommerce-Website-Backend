using Ecommerce_Website_Backend.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website_Backend.Models.Request
{
    public class ProductRequest
    {
        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(ValidationConstants.Product.ProductNameMaxLength,
            ErrorMessage = "Product name cannot exceed 100 characters")]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(ValidationConstants.Product.ProductDescriptionMaxLength,
            ErrorMessage = "Description cannot exceed 500 characters")]
        public string ProductDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "SKU is required")]
        [MaxLength(ValidationConstants.Product.SkuMaxLength,
            ErrorMessage = "SKU cannot exceed 50 characters")]
        public string Sku { get; set; } = string.Empty;

        [Range(ValidationConstants.Product.PriceMin, ValidationConstants.Product.PriceMax,
            ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "ProductCategoryId is required")]
        public int ProductCategoryId { get; set; }

        // Which merchant this product belongs to. Only used when the caller is
        // Admin/SuperAdmin — a Merchant's own value here is ignored (see ProductService).
        public int? MerchantId { get; set; }
    }
}
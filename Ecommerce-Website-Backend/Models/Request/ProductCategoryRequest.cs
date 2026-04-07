using Ecommerce_Website_Backend.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Website_Backend.Models.Request
{
    public class ProductCategoryRequest
    {
        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(ValidationConstants.ProductCategory.ProductCategoryNameMaxLength,
            ErrorMessage = "Category name cannot exceed 15 characters")]
        public string ProductCategoryName { get; set; } = string.Empty;

        [MaxLength(ValidationConstants.ProductCategory.ProductCategoryDescriptionMaxLength,
            ErrorMessage = "Description cannot exceed 50 characters")]
        public string ProductCategoryDescription { get; set; } = string.Empty;
    }
}

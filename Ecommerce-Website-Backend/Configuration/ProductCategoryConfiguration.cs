using Ecommerce_Website_Backend.Common.Constants;
using Ecommerce_Website_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_Website_Backend.Configuration
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryEntity> builder)
        {
            builder.HasIndex(c => c.ProductCategoryName)
                .IsUnique();

            builder.Property(c => c.ProductCategoryName)
                .HasMaxLength(ValidationConstants.ProductCategory.ProductCategoryNameMaxLength);

            builder.Property(c => c.ProductCategoryDescription)
                .HasMaxLength(ValidationConstants.ProductCategory.ProductCategoryDescriptionMaxLength);
        }
    }
}

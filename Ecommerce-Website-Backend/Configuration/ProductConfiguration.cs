using Ecommerce_Website_Backend.Common.Constants;
using Ecommerce_Website_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_Website_Backend.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasIndex(p => p.Sku)
                .IsUnique();

            builder.Property(p => p.ProductName)
                .HasMaxLength(ValidationConstants.Product.ProductNameMaxLength);

            builder.Property(p => p.ProductDescription)
                .HasMaxLength(ValidationConstants.Product.ProductDescriptionMaxLength);

            builder.Property(p => p.Sku)
                .HasMaxLength(ValidationConstants.Product.SkuMaxLength);

            // decimal(18,2) — explicit precision avoids EF's silent truncation warning on money columns
            builder.Property(p => p.Price)
                .HasPrecision(18, 2);

            // block deleting a category that still has products
            builder.HasOne(p => p.ProductCategory)
                .WithMany()
                .HasForeignKey(p => p.ProductCategoryId)
                .OnDelete(DeleteBehavior.Restrict); 

            // MerchantId is nullable — Admin/SuperAdmin can own products directly
            // block deleting a merchant account that still has products
            builder.HasOne(p => p.Merchant)
                .WithMany()
                .HasForeignKey(p => p.MerchantId)
                .IsRequired(false) 
                .OnDelete(DeleteBehavior.Restrict); 

        }
    }
}

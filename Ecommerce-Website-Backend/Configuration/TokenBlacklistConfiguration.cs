using Ecommerce_Website_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_Website_Backend.Configuration
{
    public class TokenBlacklistConfiguration : IEntityTypeConfiguration<TokenBlacklistEntity>
    {
        public void Configure(EntityTypeBuilder<TokenBlacklistEntity> builder)
        {
            builder.HasIndex(t => t.SessionToken)
                .IsUnique();
        }
    }
}

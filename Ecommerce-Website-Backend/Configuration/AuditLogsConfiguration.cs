using Ecommerce_Website_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_Website_Backend.Configuration
{
    public class AuditLogsConfiguration : IEntityTypeConfiguration<AuditLogsEntity>
    {
        public void Configure(EntityTypeBuilder<AuditLogsEntity> builder)
        {
            builder.Property(a => a.EntityName)
                .HasMaxLength(100);

            builder.Property(a => a.Action)
                .HasMaxLength(20);

            builder.Property(a => a.ChangedBy)
                .HasMaxLength(50);
        }
    }
}

using Ecommerce_Website_Backend.Common.Constants;
using Ecommerce_Website_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Ecommerce_Website_Backend.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        //My Database Tables
        public DbSet<ProductCategoryEntity> ProductCategories => Set<ProductCategoryEntity>();
        public DbSet<AuditLogsEntity> AuditLogs => Set<AuditLogsEntity>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // ProductCategoryName must be unique — no duplicate category names
            builder.Entity<ProductCategoryEntity>()
                .HasIndex(c => c.ProductCategoryName)
                .IsUnique();

            // ValidationConstants
            builder.Entity<ProductCategoryEntity>()
                .Property(c => c.ProductCategoryName)
                .HasMaxLength(ValidationConstants.ProductCategory.ProductCategoryNameMaxLength);

            builder.Entity<ProductCategoryEntity>()
                .Property(c => c.ProductCategoryDescription)
                .HasMaxLength(ValidationConstants.ProductCategory.ProductCategoryDescriptionMaxLength);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var auditEntries = new List<AuditLogsEntity>();

            foreach (var entry in ChangeTracker.Entries())
            {
                // Skip AuditLogsEntity — avoid infinite loop
                if (entry.Entity is AuditLogsEntity) continue;

                // Skip unchanged entries
                if (entry.State is EntityState.Detached or EntityState.Unchanged) continue;

                auditEntries.Add(new AuditLogsEntity
                {
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = entry.Properties
                                .FirstOrDefault(p => p.Metadata.IsPrimaryKey())
                                ?.CurrentValue?.ToString() ?? string.Empty,
                    Action = entry.State switch
                    {
                        EntityState.Added => "Created",
                        EntityState.Modified => "Updated",
                        EntityState.Deleted => "Deleted",
                        _ => "Unknown"
                    },
                    OldValues = entry.State == EntityState.Modified
                                ? JsonSerializer.Serialize(
                                    entry.Properties.ToDictionary(
                                        p => p.Metadata.Name,
                                        p => p.OriginalValue))
                                : null,
                    NewValues = entry.State != EntityState.Deleted
                                ? JsonSerializer.Serialize(
                                    entry.Properties.ToDictionary(
                                        p => p.Metadata.Name,
                                        p => p.CurrentValue))
                                : null,
                    ChangedBy = null, // fill once auth is implemented
                    ChangedAt = DateTime.UtcNow
                });
            }

            // Save main changes first
            var result = await base.SaveChangesAsync(ct);

            // Then save audit logs
            if (auditEntries.Count != 0)
            {
                AuditLogs.AddRange(auditEntries);
                await base.SaveChangesAsync(ct);
            }

            return result;
        }
    }
}

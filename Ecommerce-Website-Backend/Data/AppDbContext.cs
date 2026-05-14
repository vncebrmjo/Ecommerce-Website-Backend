using Ecommerce_Website_Backend.Common.Constants;
using Ecommerce_Website_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace Ecommerce_Website_Backend.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
    {
        //My Database Tables
        public DbSet<ProductCategoryEntity> ProductCategories => Set<ProductCategoryEntity>();
        public DbSet<AuditLogsEntity> AuditLogs => Set<AuditLogsEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();

        public DbSet<TokenBlacklistEntity> TokenBlacklist => Set<TokenBlacklistEntity>();

        private string? CurrentUser =>
            httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub) is { } sub
                ? httpContextAccessor.HttpContext?.User.FindFirst("userName")?.Value
                  ?? sub.Value  // fallback to user Id if username claim not found
                : null;


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

            builder.Entity<UserEntity>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            builder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<UserEntity>()
                .Property(u => u.UserName)
                .HasMaxLength(ValidationConstants.User.UserNameMaxLength);

            builder.Entity<UserEntity>()
                .Property(u => u.Email)
                .HasMaxLength(ValidationConstants.User.EmailMaxLength);

            builder.Entity<UserEntity>()
                .Property(u => u.FirstName)
                .HasMaxLength(ValidationConstants.User.FirstNameMaxLength);

            builder.Entity<UserEntity>()
                .Property(u => u.LastName)
                .HasMaxLength(ValidationConstants.User.LastNameMaxLength);

            builder.Entity<UserEntity>()
                .Property(u => u.Role)
                .HasMaxLength(20);

            builder.Entity<TokenBlacklistEntity>()
                .HasIndex(t => t.SessionToken)
                .IsUnique();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var auditEntries = new List<AuditLogsEntity>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLogsEntity) continue;
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
                                    entry.Properties
                                        .Where(p => p.Metadata.Name != nameof(UserEntity.PasswordHash))
                                        .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue))
                                : null,
                    ChangedBy = CurrentUser,
                    ChangedAt = DateTime.UtcNow
                });
            }

            var result = await base.SaveChangesAsync(ct);

            if (auditEntries.Count != 0)
            {
                AuditLogs.AddRange(auditEntries);
                await base.SaveChangesAsync(ct);
            }

            return result;
        }
    }
}

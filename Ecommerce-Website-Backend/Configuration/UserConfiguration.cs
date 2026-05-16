using Ecommerce_Website_Backend.Common.Constants;
using Ecommerce_Website_Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce_Website_Backend.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasIndex(u => u.UserName)
                .IsUnique();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.UserName)
                .HasMaxLength(ValidationConstants.User.UserNameMaxLength);

            builder.Property(u => u.Email)
                .HasMaxLength(ValidationConstants.User.EmailMaxLength);

            builder.Property(u => u.FirstName)
                .HasMaxLength(ValidationConstants.User.FirstNameMaxLength);

            builder.Property(u => u.LastName)
                .HasMaxLength(ValidationConstants.User.LastNameMaxLength);

            builder.Property(u => u.Role)
                .HasMaxLength(20);
        }
    }
}

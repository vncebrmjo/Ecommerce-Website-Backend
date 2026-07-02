using Ecommerce_Website_Backend.Common.Constants;
using Ecommerce_Website_Backend.Data;
using Ecommerce_Website_Backend.Models.Request;
using Ecommerce_Website_Backend.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Website_Backend.Services
{
    public class UserManagementService(AppDbContext db)
    {
        public async Task<List<UserResponse>> GetAllAsync()
        {
            return await db.Users
                .Select(u => UserResponse.FromEntity(u))
                .ToListAsync();
        }

        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var user = await db.Users.FindAsync(id)
                ?? throw new KeyNotFoundException($"User with Id {id} not found.");

            return UserResponse.FromEntity(user);
        }

        public async Task<UserResponse> UpdateRoleAsync(int id, UpdateUserRoleRequest request, string requestingUserRole)
        {
            if (!request.IsValidRole())
                throw new InvalidOperationException(
                    $"Invalid role. Valid roles are: {string.Join(", ", UserRoles.All)}");

            var user = await db.Users.FindAsync(id)
                ?? throw new KeyNotFoundException($"User with Id {id} not found.");

            // Prevent changing your own role
            if (user.Role == UserRoles.SuperAdmin && requestingUserRole != UserRoles.SuperAdmin)
                throw new UnauthorizedAccessException(
                    "Only a SuperAdmin can modify another SuperAdmin's role.");

            // Prevent assigning SuperAdmin role unless you are SuperAdmin
            if (request.Role == UserRoles.SuperAdmin && requestingUserRole != UserRoles.SuperAdmin)
                throw new UnauthorizedAccessException(
                    "Only a SuperAdmin can assign the SuperAdmin role.");

            user.Role = request.Role;
            await db.SaveChangesAsync();

            return UserResponse.FromEntity(user);
        }

        public async Task<UserResponse> ToggleStatusAsync(int id, string requestingUserRole)
        {
            var user = await db.Users.FindAsync(id)
                ?? throw new KeyNotFoundException($"User with Id {id} not found.");

            // Only SuperAdmin can deactivate an Admin
            if (user.Role == UserRoles.Admin && requestingUserRole != UserRoles.SuperAdmin)
                throw new UnauthorizedAccessException(
                    "Only a SuperAdmin can deactivate an Admin account.");

            // Nobody can deactivate a SuperAdmin
            if (user.Role == UserRoles.SuperAdmin)
                throw new UnauthorizedAccessException(
                    "SuperAdmin accounts cannot be deactivated.");

            user.IsActive = !user.IsActive;
            await db.SaveChangesAsync();

            return UserResponse.FromEntity(user);
        }

        public async Task DeleteAsync(int id, string requestingUserRole)
        {
            var user = await db.Users.FindAsync(id)
                ?? throw new KeyNotFoundException($"User with Id {id} not found.");

            // Only SuperAdmin can delete an Admin
            if (user.Role == UserRoles.Admin && requestingUserRole != UserRoles.SuperAdmin)
                throw new UnauthorizedAccessException(
                    "Only a SuperAdmin can delete an Admin account.");

            // Nobody can delete a SuperAdmin
            if (user.Role == UserRoles.SuperAdmin)
                throw new UnauthorizedAccessException(
                    "SuperAdmin accounts cannot be deleted.");

            db.Users.Remove(user);
            await db.SaveChangesAsync();
        }
    }
}

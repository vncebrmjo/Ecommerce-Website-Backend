using Ecommerce_Website_Backend.Data;
using Ecommerce_Website_Backend.Data.Entities;
using Ecommerce_Website_Backend.Models.Request;
using Ecommerce_Website_Backend.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Website_Backend.Services
{
    public class ProductCategoryService(AppDbContext db)
    {
        // Get all categories
        public async Task<List<ProductCategoryResponse>> GetAllAsync()
        {
            return await db.ProductCategories
                .Select(c => ProductCategoryResponse.FromEntity(c))
                .ToListAsync();
        }

        // Get single category by Id
        public async Task<ProductCategoryResponse> GetByIdAsync(int id)
        {
            var category = await db.ProductCategories.FindAsync(id)
                ?? throw new KeyNotFoundException($"Category with Id {id} not found.");

            return ProductCategoryResponse.FromEntity(category);
        }

        // Create new category
        public async Task<ProductCategoryResponse> CreateAsync(ProductCategoryRequest request)
        {
            // Business rule — no duplicate category names
            var exists = await db.ProductCategories
                .AnyAsync(c => c.ProductCategoryName == request.ProductCategoryName);

            if (exists)
                throw new InvalidOperationException($"Category '{request.ProductCategoryName}' already exists.");

            var category = new ProductCategoryEntity
            {
                ProductCategoryName = request.ProductCategoryName,
                ProductCategoryDescription = request.ProductCategoryDescription
            };

            db.ProductCategories.Add(category);
            await db.SaveChangesAsync();

            return ProductCategoryResponse.FromEntity(category);
        }

        // Update existing category
        public async Task<ProductCategoryResponse> UpdateAsync(int id, ProductCategoryRequest request)
        {
            var category = await db.ProductCategories.FindAsync(id)
                ?? throw new KeyNotFoundException($"Category with Id {id} not found.");

            // Business rule — check duplicate only if name actually changed
            if (category.ProductCategoryName != request.ProductCategoryName)
            {
                var exists = await db.ProductCategories
                    .AnyAsync(c => c.ProductCategoryName == request.ProductCategoryName);

                if (exists)
                    throw new InvalidOperationException($"Category '{request.ProductCategoryName}' already exists.");
            }

            category.ProductCategoryName = request.ProductCategoryName;
            category.ProductCategoryDescription = request.ProductCategoryDescription;

            await db.SaveChangesAsync();

            return ProductCategoryResponse.FromEntity(category);
        }

        // Delete category
        public async Task DeleteAsync(int id)
        {
            var category = await db.ProductCategories.FindAsync(id)
                ?? throw new KeyNotFoundException($"Category with Id {id} not found.");

            db.ProductCategories.Remove(category);
            await db.SaveChangesAsync();
        }
    }

}

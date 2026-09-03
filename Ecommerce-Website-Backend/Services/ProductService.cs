using Ecommerce_Website_Backend.Common.Constants;
using Ecommerce_Website_Backend.Data;
using Ecommerce_Website_Backend.Data.Entities;
using Ecommerce_Website_Backend.Models.Request;
using Ecommerce_Website_Backend.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Website_Backend.Services
{
    public class ProductService(AppDbContext db)
    {
        // Get all products (read-only — no tracking needed)
        public async Task<List<ProductResponse>> GetAllAsync(CancellationToken ct)
        {
            return await db.Products
                .AsNoTracking()
                .Include(p => p.ProductCategory)
                .Include(p => p.Merchant)
                .Select(p => ProductResponse.FromEntity(p))
                .ToListAsync(ct);
        }

        // Get single product by Id (read-only — no tracking needed)
        public async Task<ProductResponse> GetByIdAsync(int id, CancellationToken ct)
        {
            var product = await db.Products
                .AsNoTracking()
                .Include(p => p.ProductCategory)
                .Include(p => p.Merchant)
                .FirstOrDefaultAsync(p => p.Id == id, ct)
                ?? throw new KeyNotFoundException($"Product with Id {id} not found.");

            return ProductResponse.FromEntity(product);
        }

        // Create new product
        public async Task<ProductResponse> CreateAsync(
            ProductRequest request, int callerId, string callerRole, CancellationToken ct)
        {
            var categoryExists = await db.ProductCategories
                .AnyAsync(c => c.Id == request.ProductCategoryId, ct);

            if (!categoryExists)
                throw new KeyNotFoundException($"Category with Id {request.ProductCategoryId} not found.");

            var skuExists = await db.Products
                .AnyAsync(p => p.Sku == request.Sku, ct);

            if (skuExists)
                throw new InvalidOperationException($"SKU '{request.Sku}' is already in use.");

            // Decide who owns this product based on the caller's role.
            var merchantId = await ResolveMerchantIdAsync(request.MerchantId, callerId, callerRole, ct);

            var product = new ProductEntity
            {
                ProductName = request.ProductName.Trim(),
                ProductDescription = request.ProductDescription.Trim(),
                Sku = request.Sku.Trim(),
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                ProductCategoryId = request.ProductCategoryId,
                MerchantId = merchantId
            };

            db.Products.Add(product);
            await db.SaveChangesAsync(ct);

            // Reload with category/merchant included for the response
            return await GetByIdAsync(product.Id, ct);
        }

        // Update existing product
        public async Task<ProductResponse> UpdateAsync(
            int id, ProductRequest request, int callerId, string callerRole, CancellationToken ct)
        {
            var product = await db.Products
                .FirstOrDefaultAsync(p => p.Id == id, ct)
                ?? throw new KeyNotFoundException($"Product with Id {id} not found.");

            EnsureCanModify(product, callerId, callerRole);

            if (product.ProductCategoryId != request.ProductCategoryId)
            {
                var categoryExists = await db.ProductCategories
                    .AnyAsync(c => c.Id == request.ProductCategoryId, ct);

                if (!categoryExists)
                    throw new KeyNotFoundException($"Category with Id {request.ProductCategoryId} not found.");
            }

            // Check duplicate SKU only if it actually changed
            if (product.Sku != request.Sku)
            {
                var skuExists = await db.Products
                    .AnyAsync(p => p.Sku == request.Sku, ct);

                if (skuExists)
                    throw new InvalidOperationException($"SKU '{request.Sku}' is already in use.");
            }

            // Only Admin/SuperAdmin can reassign ownership; a Merchant keeps their own MerchantId.
            var merchantId = await ResolveMerchantIdAsync(
                request.MerchantId, callerId, callerRole, ct, existingMerchantId: product.MerchantId);

            product.ProductName = request.ProductName.Trim();
            product.ProductDescription = request.ProductDescription.Trim();
            product.Sku = request.Sku.Trim();
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.ProductCategoryId = request.ProductCategoryId;
            product.MerchantId = merchantId;

            await db.SaveChangesAsync(ct);

            return await GetByIdAsync(product.Id, ct);
        }

        // Delete product
        public async Task DeleteAsync(int id, int callerId, string callerRole, CancellationToken ct)
        {
            var product = await db.Products.FindAsync([id], ct)
                ?? throw new KeyNotFoundException($"Product with Id {id} not found.");

            EnsureCanModify(product, callerId, callerRole);

            db.Products.Remove(product);
            await db.SaveChangesAsync(ct);
        }

        // Checks whether the caller owns this product. Role alone (from [Authorize])
        // isn't enough — a Merchant should only touch their own products.
        private static void EnsureCanModify(ProductEntity product, int callerId, string callerRole)
        {
            if (callerRole == UserRoles.Merchant && product.MerchantId != callerId)
                throw new UnauthorizedAccessException("You do not have permission to modify this product.");

            // Admin/SuperAdmin bypass ownership — they can moderate the whole catalog.
        }

        // Figures out who should own the product:
        // - Merchant: always themselves, ignoring whatever they sent.
        // - Admin/SuperAdmin creating: whoever they specify (must be a real Merchant), or null for platform-owned.
        // - Admin/SuperAdmin updating with no MerchantId sent: ownership stays the same.
        private async Task<int?> ResolveMerchantIdAsync(
            int? requestedMerchantId,
            int callerId,
            string callerRole,
            CancellationToken ct,
            int? existingMerchantId = null)
        {
            if (callerRole == UserRoles.Merchant)
                return callerId;

            // Admin/SuperAdmin path from here.
            if (requestedMerchantId is null)
                return existingMerchantId; // create: null → platform-owned; update: unchanged

            var merchant = await db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == requestedMerchantId, ct)
                ?? throw new KeyNotFoundException($"User with Id {requestedMerchantId} not found.");

            if (merchant.Role != UserRoles.Merchant)
                throw new InvalidOperationException(
                    $"User with Id {requestedMerchantId} is not a Merchant and cannot own products. " +
                    "Customers must register a separate Merchant account.");

            return requestedMerchantId;
        }
    }
}
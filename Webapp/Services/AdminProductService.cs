using Microsoft.EntityFrameworkCore;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;
using Webapp.Models;

namespace OnlineShopServer.Services
{
    public class AdminProductService : IAdminProductService
    {
        private readonly OnlineShopDbContext _context;
        public AdminProductService(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductAdminDTO>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(x => new ProductAdminDTO
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.CategoryName,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.SupplierName,
                    UnitPrice = x.UnitPrice,
                    UnitsInStock = x.UnitsInStock,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }

        public async Task<ProductUpdateDTO?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Where(x => x.ProductId == productId)
                .Select(x => new ProductUpdateDTO
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    CategoryId = x.CategoryId,
                    SupplierId = x.SupplierId,
                    UnitPrice = x.UnitPrice,
                    UnitsInStock = x.UnitsInStock,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(bool Success, string Message)> CreateProductAsync(ProductCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProductName))
                return (false, "Product name is required");

            if (dto.UnitPrice < 0)
                return (false, "Price must be >= 0");

            if (dto.UnitsInStock < 0)
                return (false, "Stock must be >= 0");

            var categoryExists = await _context.Categories.AnyAsync(x => x.CategoryId == dto.CategoryId);
            if (!categoryExists)
                return (false, "Category not found");

            var supplierExists = await _context.Suppliers.AnyAsync(x => x.SupplierId == dto.SupplierId);
            if (!supplierExists)
                return (false, "Supplier not found");

            var product = new Product
            {
                ProductName = dto.ProductName,
                CategoryId = dto.CategoryId,
                SupplierId = dto.SupplierId,
                UnitPrice = dto.UnitPrice,
                UnitsInStock = dto.UnitsInStock,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return (true, "Product created successfully");
        }

        public async Task<(bool Success, string Message)> UpdateProductAsync(ProductUpdateDTO dto)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == dto.ProductId);
            if (product == null)
                return (false, "Product not found");

            if (string.IsNullOrWhiteSpace(dto.ProductName))
                return (false, "Product name is required");

            if (dto.UnitPrice < 0)
                return (false, "Price must be >= 0");

            if (dto.UnitsInStock < 0)
                return (false, "Stock must be >= 0");

            var categoryExists = await _context.Categories.AnyAsync(x => x.CategoryId == dto.CategoryId);
            if (!categoryExists)
                return (false, "Category not found");

            var supplierExists = await _context.Suppliers.AnyAsync(x => x.SupplierId == dto.SupplierId);
            if (!supplierExists)
                return (false, "Supplier not found");

            product.ProductName = dto.ProductName;
            product.CategoryId = dto.CategoryId;
            product.SupplierId = dto.SupplierId;
            product.UnitPrice = dto.UnitPrice;
            product.UnitsInStock = dto.UnitsInStock;
            product.Description = dto.Description;
            product.ImageUrl = dto.ImageUrl;
            product.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return (true, "Product updated successfully");
        }

        public async Task<(bool Success, string Message)> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product == null)
                return (false, "Product not found");

            var hasCartItems = await _context.CartItems.AnyAsync(x => x.ProductId == productId);
            if (hasCartItems)
                return (false, "Cannot delete product because it exists in cart items");

            var hasOrderItems = await _context.OrderItems.AnyAsync(x => x.ProductId == productId);
            if (hasOrderItems)
                return (false, "Cannot delete product because it exists in order items");

            var hasFavorites = await _context.Favorites.AnyAsync(x => x.ProductId == productId);
            if (hasFavorites)
                return (false, "Cannot delete product because it exists in favorites");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return (true, "Product deleted successfully");
        }

        public async Task<List<SupplierDTO>> GetSuppliersAsync()
        {
            return await _context.Suppliers
                .Select(x => new SupplierDTO
                {
                    SupplierId = x.SupplierId,
                    SupplierName = x.SupplierName
                })
                .ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Webapp.DTO;
using ServerShopOnline.Services.Interface;
using Webapp.Models;

namespace ServerShopOnline.Services
{
    public class ProductService : IProductService
    {
        private readonly OnlineShopDbContext _context;
        public ProductService(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task<ProductDetailDTO?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.Include(p => p.Category).Include(p => p.Supplier)
                .Where(p => p.ProductId == productId && p.IsActive)
                .Select(p => new ProductDetailDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category.CategoryName,
                    SupplierName = p.Supplier.SupplierName
                }).FirstOrDefaultAsync();


        }

        public async Task<List<ProductListDTO>> GetProductsAsync(string? keyword, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.Include(p => p.Category).Where(p =>p.IsActive).AsQueryable();
            if(!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.ProductName.Contains(keyword));
            }
            if(categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            if(minPrice.HasValue)
            {
                query = query.Where(p => p.UnitPrice >= minPrice.Value);
            }
            if(maxPrice.HasValue)
            {
                query = query.Where(p => p.UnitPrice <= maxPrice.Value);
            }
            return await query.Select(p => new ProductListDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category.CategoryName
            }).ToListAsync();
        }

       
    }
}

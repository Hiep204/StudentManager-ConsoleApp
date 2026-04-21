using OnlineShopServer.Models;
using OnlineShopServer.Services.Interface;
using Webapp.DTO;
using Webapp.Models;

namespace OnlineShopServer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly OnlineShopDbContext _context;
        public CategoryService(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            return await Task.Run(() =>
            {
                return _context.Categories.Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                }).ToList();
            });
        }
    }
}

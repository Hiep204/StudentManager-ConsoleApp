using Webapp.DTO;

namespace OnlineShopServer.Services.Interface
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetCategoriesAsync();
    }
}

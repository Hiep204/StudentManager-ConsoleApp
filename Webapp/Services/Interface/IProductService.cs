using Webapp.DTO;

namespace ServerShopOnline.Services.Interface
{
    public interface IProductService
    {
        Task<List<ProductListDTO>> GetProductsAsync(string? keyword, int? categoryId, decimal? minPrice, decimal? maxPrice);
        Task<ProductDetailDTO?> GetProductByIdAsync(int productId);
    }
}

using Webapp.DTO;

namespace OnlineShopServer.Services.Interface
{
    public interface IAdminProductService
    {
        Task<List<ProductAdminDTO>> GetAllProductsAsync();
        Task<ProductUpdateDTO?> GetProductByIdAsync(int productId);
        Task<(bool Success, string Message)> CreateProductAsync(ProductCreateDTO dto);
        Task<(bool Success, string Message)> UpdateProductAsync(ProductUpdateDTO dto);
        Task<(bool Success, string Message)> DeleteProductAsync(int productId);
        Task<List<SupplierDTO>> GetSuppliersAsync();
    }
}

using Webapp.DTO;

namespace OnlineShopServer.Services.Interface
{
    public interface ICartService
    {
        Task<(bool Success, string Message)> AddToCartAsync(AddToCartDTO dto);
        Task<List<CartItemDTO>> GetCartItemsAsync(int userId);
        Task<(bool Success, string Message)> IncreaseQuantityAsync(int cartItemId);
        Task<(bool Success, string Message)> DecreaseQuantityAsync(int cartItemId);
        Task<(bool Success, string Message)> RemoveCartItemAsync(int cartItemId);
    }
}

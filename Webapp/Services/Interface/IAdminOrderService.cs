using Webapp.DTO;

namespace OnlineShopServer.Services.Interface
{
    public interface IAdminOrderService
    {
        Task<List<AdminOrderDTO>> GetAllOrdersAsync();
        Task<AdminOrderDTO?> GetOrderByIdAsync(int orderId);
        Task<List<AdminOrderItemDTO>> GetOrderItemsAsync(int orderId);
        Task<(bool Success, string Message)> UpdateOrderStatusAsync(UpdateOrderStatusDTO dto);
    }
}

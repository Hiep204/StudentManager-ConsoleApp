using Webapp.DTO;
using System.Threading.Tasks;

namespace ServerShopOnline.Services.Interface
{
    public interface IOrderService
    {
        Task<(bool Success, string Message, int OrderId)> CheckoutAsync(CheckoutDTO dto);
        Task<List<OrderHistoryDTO>> GetOrderHistoryAsync(int userId);
        Task<List<OrderItemDTO>> GetOrderItemsAsync(int orderId, int userId);
    }
}

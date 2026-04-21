using Webapp.DTO;


namespace ServerShopOnline.Services.Interface
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDTO> GetSummaryAsync();
        Task<List<TopProductDTO>> GetTopSellingProductsAsync(int top);
        Task<List<OrderStatusCountDTO>> GetOrderStatusCountsAsync();
        Task<List<LowStockProductDTO>> GetLowStockProductsAsync(int threshold);
        Task<List<RevenueByMonthDTO>> GetRevenueByMonthAsync(int year);
    }
}

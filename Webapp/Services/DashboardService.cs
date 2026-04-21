using Microsoft.EntityFrameworkCore;
using Webapp.DTO;
using ServerShopOnline.Services.Interface;
using Webapp.DTO;
using Webapp.Models;

namespace ServerShopOnline.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly OnlineShopDbContext _context;
        public DashboardService(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task<DashboardSummaryDTO> GetSummaryAsync()
        {
            var now = DateTime.Now;
            var month = now.Month;
            var year = now.Year;

            var revenueThisMonth = await _context.Orders
                .Where(x => x.OrderDate.Year == year
                         && x.OrderDate.Month == month
                         && x.Status != "Cancelled")
                .SumAsync(x => (decimal?)x.TotalAmount) ?? 0;

            var revenueThisYear = await _context.Orders
                .Where(x => x.OrderDate.Year == year
                         && x.Status != "Cancelled")
                .SumAsync(x => (decimal?)x.TotalAmount) ?? 0;

            return new DashboardSummaryDTO
            {
                TotalProducts = await _context.Products.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalCustomers = await _context.Users.CountAsync(x => x.Role.RoleName == "Customer"),
                RevenueThisMonth = revenueThisMonth,
                RevenueThisYear = revenueThisYear
            };
        }

        public async Task<List<TopProductDTO>> GetTopSellingProductsAsync(int top)
        {
            return await _context.OrderItems
                .GroupBy(x => new { x.ProductId, x.Product.ProductName, x.Product.ImageUrl })
                .Select(g => new TopProductDTO
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    ImageUrl = g.Key.ImageUrl,
                    TotalQuantitySold = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.LineTotal)
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .Take(top)
                .ToListAsync();
        }

        public async Task<List<OrderStatusCountDTO>> GetOrderStatusCountsAsync()
        {
            return await _context.Orders
                .GroupBy(x => x.Status)
                .Select(g => new OrderStatusCountDTO
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        public async Task<List<LowStockProductDTO>> GetLowStockProductsAsync(int threshold)
        {
            return await _context.Products
                .Where(x => x.IsActive && x.UnitsInStock <= threshold)
                .OrderBy(x => x.UnitsInStock)
                .Select(x => new LowStockProductDTO
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    UnitsInStock = x.UnitsInStock
                })
                .ToListAsync();
        }

        public async Task<List<RevenueByMonthDTO>> GetRevenueByMonthAsync(int year)
        {
            var rawData = await _context.Orders
                .Where(x => x.OrderDate.Year == year && x.Status != "Cancelled")
                .GroupBy(x => x.OrderDate.Month)
                .Select(g => new RevenueByMonthDTO
                {
                    Month = g.Key,
                    Revenue = g.Sum(x => x.TotalAmount)
                })
                .ToListAsync();

            var result = new List<RevenueByMonthDTO>();

            for (int month = 1; month <= 12; month++)
            {
                var found = rawData.FirstOrDefault(x => x.Month == month);
                result.Add(new RevenueByMonthDTO
                {
                    Month = month,
                    Revenue = found?.Revenue ?? 0
                });
            }

            return result;
        }


    }
}

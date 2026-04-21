using Microsoft.EntityFrameworkCore;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;
using Webapp.Models;

namespace OnlineShopServer.Services
{
    public class AdminOrderService : IAdminOrderService
    {
        private readonly OnlineShopDbContext _context;
        public AdminOrderService(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task<List<AdminOrderDTO>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .OrderByDescending(x => x.OrderDate)
                .Select(x => new AdminOrderDTO
                {
                    OrderId = x.OrderId,
                    UserId = x.UserId,
                    CustomerName = x.User.FullName,
                    OrderDate = x.OrderDate,
                    Status = x.Status,
                    PaymentMethod = x.PaymentMethod,
                    PaymentStatus = x.PaymentStatus,
                    ReceiverName = x.ReceiverName,
                    ReceiverPhone = x.ReceiverPhone,
                    ShippingAddress = x.ShippingAddress,
                    SubTotal = x.SubTotal,
                    ShippingFee = x.ShippingFee,
                    TotalAmount = x.TotalAmount
                })
                .ToListAsync();
        }

        public async Task<AdminOrderDTO?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Where(x => x.OrderId == orderId)
                .Select(x => new AdminOrderDTO
                {
                    OrderId = x.OrderId,
                    UserId = x.UserId,
                    CustomerName = x.User.FullName,
                    OrderDate = x.OrderDate,
                    Status = x.Status,
                    PaymentMethod = x.PaymentMethod,
                    PaymentStatus = x.PaymentStatus,
                    ReceiverName = x.ReceiverName,
                    ReceiverPhone = x.ReceiverPhone,
                    ShippingAddress = x.ShippingAddress,
                    SubTotal = x.SubTotal,
                    ShippingFee = x.ShippingFee,
                    TotalAmount = x.TotalAmount
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<AdminOrderItemDTO>> GetOrderItemsAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(x => x.OrderId == orderId)
                .Select(x => new AdminOrderItemDTO
                {
                    OrderItemId = x.OrderItemId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.ProductName,
                    ImageUrl = x.Product.ImageUrl,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    Discount = x.Discount,
                    LineTotal = x.LineTotal
                })
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> UpdateOrderStatusAsync(UpdateOrderStatusDTO dto)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == dto.OrderId);
            if (order == null)
                return (false, "Order not found");

            if (string.IsNullOrWhiteSpace(dto.NewStatus))
                return (false, "New status is required");

            var validStatuses = new[] { "Pending", "Confirmed", "Shipping", "Completed", "Cancelled" };
            if (!validStatuses.Contains(dto.NewStatus))
                return (false, "Invalid status");

            var oldStatus = order.Status;

            order.Status = dto.NewStatus;

            if (dto.NewStatus == "Completed")
            {
                order.PaymentStatus = "Paid";

                var payment = await _context.Payments.FirstOrDefaultAsync(x => x.OrderId == order.OrderId);
                if (payment != null)
                {
                    payment.Status = "Paid";
                    payment.PaidAt = DateTime.Now;
                }
            }

            if (dto.NewStatus == "Cancelled")
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(x => x.OrderId == order.OrderId);
                if (payment != null && payment.Status != "Paid")
                {
                    payment.Status = "Cancelled";
                }
            }

            var history = new OrderStatusHistory
            {
                OrderId = order.OrderId,
                OldStatus = oldStatus,
                NewStatus = dto.NewStatus,
                ChangedBy = dto.ChangedBy,
                ChangedAt = DateTime.Now,
                Note = dto.Note
            };

            _context.OrderStatusHistories.Add(history);
            await _context.SaveChangesAsync();

            return (true, "Order status updated successfully");
        }
    }
}

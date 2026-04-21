using Microsoft.EntityFrameworkCore;
using Webapp.DTO;
using ServerShopOnline.Services.Interface;
using Webapp.Models;

namespace OnlineShopServer.Services
{
    public class OrderService : IOrderService
    {
        private readonly OnlineShopDbContext _context;
        public OrderService(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task<(bool Success, string Message, int OrderId)> CheckoutAsync(CheckoutDTO dto)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == dto.UserId);
            if (cart == null)
                return (false, "Cart not found", 0);

            var cartItems = await _context.CartItems
                .Where(x => x.CartId == cart.CartId)
                .Include(x => x.Product)
                .ToListAsync();

            if (!cartItems.Any())
                return (false, "Cart is empty", 0);

            if (string.IsNullOrWhiteSpace(dto.ReceiverName) ||
                string.IsNullOrWhiteSpace(dto.ReceiverPhone) ||
                string.IsNullOrWhiteSpace(dto.ShippingAddress) ||
                string.IsNullOrWhiteSpace(dto.PaymentMethod))
            {
                return (false, "Checkout information is required", 0);
            }

            decimal subTotal = cartItems.Sum(x => x.UnitPrice * x.Quantity);
            decimal shippingFee = 30000;
            decimal totalAmount = subTotal + shippingFee;

            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = dto.PaymentMethod == "COD" ? "Unpaid" : "Pending",
                ReceiverName = dto.ReceiverName,
                ReceiverPhone = dto.ReceiverPhone,
                ShippingAddress = dto.ShippingAddress,
                Note = dto.Note,
                SubTotal = subTotal,
                ShippingFee = shippingFee,
                TotalAmount = totalAmount
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Discount = 0,
                    LineTotal = item.UnitPrice * item.Quantity
                };

                _context.OrderItems.Add(orderItem);
            }

            var payment = new Payment
            {
                OrderId = order.OrderId,
                PaymentMethod = dto.PaymentMethod,
                Amount = totalAmount,
                TransactionCode = null,
                PaidAt = null,
                Status = dto.PaymentMethod == "COD" ? "Pending" : "Pending"
            };
            _context.Payments.Add(payment);

            var history = new OrderStatusHistory
            {
                OrderId = order.OrderId,
                OldStatus = null,
                NewStatus = "Pending",
                ChangedBy = dto.UserId,
                ChangedAt = DateTime.Now,
                Note = "Order created"
            };
            _context.OrderStatusHistories.Add(history);

            _context.CartItems.RemoveRange(cartItems);
            cart.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return (true, "Checkout successfully", order.OrderId);
        }

        public async Task<List<OrderHistoryDTO>> GetOrderHistoryAsync(int userId)
        {
            return await _context.Orders
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .Select(x => new OrderHistoryDTO
                {
                    OrderId = x.OrderId,
                    OrderDate = x.OrderDate,
                    Status = x.Status,
                    PaymentMethod = x.PaymentMethod,
                    PaymentStatus = x.PaymentStatus,
                    SubTotal = x.SubTotal,
                    ShippingFee = x.ShippingFee,
                    TotalAmount = x.TotalAmount
                })
                .ToListAsync();
        }

        public async Task<List<OrderItemDTO>> GetOrderItemsAsync(int orderId, int userId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(x => x.OrderId == orderId && x.UserId == userId);

            if (order == null)
                return new List<OrderItemDTO>();

            return await _context.OrderItems
                .Where(x => x.OrderId == orderId)
                .Select(x => new OrderItemDTO
                {
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
    }
}

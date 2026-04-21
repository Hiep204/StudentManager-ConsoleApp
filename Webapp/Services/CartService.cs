using Microsoft.EntityFrameworkCore;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;
using Webapp.Models;

namespace OnlineShopServer.Services
{
    public class CartService : ICartService
    {
        private readonly OnlineShopDbContext _context;
        public CartService(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task<(bool Success, string Message)> AddToCartAsync(AddToCartDTO dto)
        {
            if (dto.Quantity <= 0)
                return (false, "Quantity must be greater than 0");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == dto.UserId);
            if (user == null)
                return (false, "User not found");

            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == dto.ProductId);
            if (product == null)
                return (false, "Product not found");

            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == dto.UserId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = dto.UserId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(x => x.CartId == cart.CartId && x.ProductId == dto.ProductId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = product.UnitPrice
                };

                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += dto.Quantity;
            }

            cart.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return (true, "Product added to cart successfully");
        }

        public async Task<(bool Success, string Message)> DecreaseQuantityAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.CartItemId == cartItemId);

            if (cartItem == null)
                return (false, "Cart item not found");

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity -= 1;
            }
            else
            {
                _context.CartItems.Remove(cartItem);
            }

            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.CartId == cartItem.CartId);
            if (cart != null)
                cart.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return (true, "Quantity decreased");
        }


        public async Task<List<CartItemDTO>> GetCartItemsAsync(int userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
                return new List<CartItemDTO>();

            return await _context.CartItems
                .Where(x => x.CartId == cart.CartId)
                .Select(x => new CartItemDTO
                {
                    CartItemId = x.CartItemId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.ProductName,
                    ImageUrl = x.Product.ImageUrl,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    Total = x.UnitPrice * x.Quantity
                })
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> IncreaseQuantityAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.CartItemId == cartItemId);

            if (cartItem == null)
                return (false, "Cart item not found");

            cartItem.Quantity += 1;

            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.CartId == cartItem.CartId);
            if (cart != null)
                cart.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return (true, "Quantity increased");
        }

        public async Task<(bool Success, string Message)> RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.CartItemId == cartItemId);

            if (cartItem == null)
                return (false, "Cart item not found");

            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.CartId == cartItem.CartId);

            _context.CartItems.Remove(cartItem);

            if (cart != null)
                cart.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return (true, "Cart item removed");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;

namespace Webapp.Pages
{
    public class CartModel : PageModel
    {
        private readonly ICartService _cartService;

        public CartModel(ICartService cartService)
        {
            _cartService = cartService;
        }

        public List<CartItemDTO> CartItems { get; set; } = new();

        public decimal GrandTotal => CartItems.Sum(x => x.Total);

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            CartItems = await _cartService.GetCartItemsAsync(userId.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostIncreaseAsync(int cartItemId)
        {
            await _cartService.IncreaseQuantityAsync(cartItemId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDecreaseAsync(int cartItemId)
        {
            await _cartService.DecreaseQuantityAsync(cartItemId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int cartItemId)
        {
            await _cartService.RemoveCartItemAsync(cartItemId);
            return RedirectToPage();
        }
    }
}

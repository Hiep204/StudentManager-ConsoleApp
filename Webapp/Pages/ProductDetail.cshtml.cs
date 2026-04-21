using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.DTO;
using ServerShopOnline.Services.Interface;
using OnlineShopServer.Services.Interface;

namespace Webapp.Pages
{
    public class ProductDetailModel : PageModel
    {
        [TempData]
        public string? Message { get; set; }

        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public ProductDetailModel(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public ProductDetailDTO? Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productService.GetProductByIdAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            var dto = new AddToCartDTO
            {
                UserId = userId.Value,
                ProductId = productId,
                Quantity = quantity
            };

            var result = await _cartService.AddToCartAsync(dto);

            if (!result.Success)
            {
                Message = "Add to cart failed: " + result.Message;
                return RedirectToPage(new { id = productId });
            }

            Message = "Product added to cart successfully";
            return RedirectToPage(new { id = productId });
        }
    }
}

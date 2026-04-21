using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.DTO;
using ServerShopOnline.Services.Interface;
using OnlineShopServer.Services.Interface;

namespace Webapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;

        public List<CategoryDTO> Categories { get; set; } = new();
        public List<ProductListDTO> Products { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? MinPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? MaxPrice { get; set; }

        [TempData]
        public string? Message { get; set; }

        public IndexModel(IProductService productService, ICategoryService categoryService, ICartService cartService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _cartService = cartService;
        }

        public async Task OnGetAsync()
        {
            Categories = await _categoryService.GetCategoriesAsync();
            Products = await _productService.GetProductsAsync(Name, Category, MinPrice, MaxPrice);
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            var dto = new AddToCartDTO
            {
                UserId = userId.Value,
                ProductId = productId,
                Quantity = 1
            };

            var result = await _cartService.AddToCartAsync(dto);

            if (!result.Success)
            {
                Message = "Add to cart failed: " + result.Message;
                return RedirectToPage();
            }

            Message = "Product added to cart successfully";
            return RedirectToPage();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.DTO;
using ServerShopOnline.Services.Interface;

namespace Webapp.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly IOrderService _orderService;

        public CheckoutModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public CheckoutDTO CheckoutData { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");

            CheckoutData.PaymentMethod = "COD";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");

            CheckoutData.UserId = userId.Value;

            var result = await _orderService.CheckoutAsync(CheckoutData);

            if (!result.Success)
            {
                ErrorMessage = result.Message;
                return Page();
            }

            TempData["Message"] = "Order placed successfully";
            return RedirectToPage("/History");
        }
    }
}
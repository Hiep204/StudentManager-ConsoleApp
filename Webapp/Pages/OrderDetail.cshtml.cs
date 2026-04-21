using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.DTO;
using ServerShopOnline.Services.Interface;

namespace Webapp.Pages
{
    public class OrderDetailModel : PageModel
    {
        private readonly IOrderService _orderService;

        public OrderDetailModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public List<OrderItemDTO> Items { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");

            Items = await _orderService.GetOrderItemsAsync(orderId, userId.Value);

            return Page();
        }
    }
}

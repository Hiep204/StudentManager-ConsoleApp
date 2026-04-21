using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;

namespace Webapp.Pages
{
    public class ManageOrderDetailModel : PageModel
    {
        private readonly IAdminOrderService _adminOrderService;

        public ManageOrderDetailModel(IAdminOrderService adminOrderService)
        {
            _adminOrderService = adminOrderService;
        }

        public AdminOrderDTO? Order { get; set; }
        public List<AdminOrderItemDTO> Items { get; set; } = new();

        [BindProperty]
        public string NewStatus { get; set; } = "";

        [BindProperty]
        public string? Note { get; set; }

        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin")
                return RedirectToPage("/Index");

            Order = await _adminOrderService.GetOrderByIdAsync(id);
            Items = await _adminOrderService.GetOrderItemsAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            var adminId = HttpContext.Session.GetInt32("UserId");

            if (role != "Admin" || adminId == null)
                return RedirectToPage("/Index");

            var dto = new UpdateOrderStatusDTO
            {
                OrderId = id,
                NewStatus = NewStatus,
                ChangedBy = adminId.Value,
                Note = Note
            };

            var result = await _adminOrderService.UpdateOrderStatusAsync(dto);

            if (!result.Success)
            {
                ErrorMessage = result.Message;
            }
            else
            {
                Message = "Order status updated successfully";
            }

            return await OnGetAsync(id);
        }
    }
}

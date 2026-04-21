using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;

namespace Webapp.Pages
{
    public class ManagerOrderModel : PageModel
    {
        private readonly IAdminOrderService _adminOrderService;

        public ManagerOrderModel(IAdminOrderService adminOrderService)
        {
            _adminOrderService = adminOrderService;
        }

        public List<AdminOrderDTO> Orders { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin")
                return RedirectToPage("/Index");

            Orders = await _adminOrderService.GetAllOrdersAsync();

            return Page();
        }
    }
}

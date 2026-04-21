using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;

namespace Webapp.Pages
{
    public class ManageProductModel : PageModel
    {
        private readonly IAdminProductService _adminProductService;

        public ManageProductModel(IAdminProductService adminProductService)
        {
            _adminProductService = adminProductService;
        }

        public List<ProductAdminDTO> Products { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin")
                return RedirectToPage("/Index");

            Products = await _adminProductService.GetAllProductsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin")
                return RedirectToPage("/Index");

            await _adminProductService.DeleteProductAsync(id);

            return RedirectToPage();
        }
    }
}

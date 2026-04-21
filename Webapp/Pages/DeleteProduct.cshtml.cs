using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShopServer.Services.Interface;

namespace Webapp.Pages
{
    public class DeleteProductModel : PageModel
    {
        private readonly IAdminProductService _adminProductService;

        public DeleteProductModel(IAdminProductService adminProductService)
        {
            _adminProductService = adminProductService;
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var role = HttpContext.Session.GetString("RoleName");
            if (role != "Admin")
                return RedirectToPage("/Index");

            var result = await _adminProductService.DeleteProductAsync(id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToPage();
            }

            TempData["Message"] = "Product deleted successfully";
            return RedirectToPage();
        }
    }
}

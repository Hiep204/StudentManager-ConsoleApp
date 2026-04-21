using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;

namespace Webapp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var dto = new LoginDTO
            {
                Email = Email,
                Password = Password
            };

            var result = await _authService.LoginAsync(dto);

            if (result == null)
            {
                ErrorMessage = "Fail email or password";
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", result.UserId);
            HttpContext.Session.SetString("FullName", result.FullName);
            HttpContext.Session.SetString("Email", result.Email);
            HttpContext.Session.SetInt32("RoleId", result.RoleId);
            HttpContext.Session.SetString("RoleName", result.RoleName);

            return RedirectToPage("/Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;

namespace Webapp.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IAuthService _authService;

        public RegisterModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string FullName { get; set; } = "";

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string? Phone { get; set; }

        [BindProperty]
        public string Password { get; set; } = "";

        [BindProperty]
        public string ConfirmPassword { get; set; } = "";

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var dto = new RegisterDTO
            {
                FullName = FullName,
                Email = Email,
                Phone = Phone,
                Password = Password,
                ConfirmPassword = ConfirmPassword
            };

            var error = await _authService.RegisterAsync(dto);

            if (error != null)
            {
                ErrorMessage = error;
                return Page();
            }

            SuccessMessage = "Register successful. Please login.";
            return RedirectToPage("/Login");
        }
    }
}
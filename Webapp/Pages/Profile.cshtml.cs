using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;

namespace Webapp.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserAddressService _userAddressService;

        public ProfileModel(IUserService userService, IUserAddressService userAddressService)
        {
            _userService = userService;
            _userAddressService = userAddressService;
        }

        [BindProperty]
        public UpdateProfileDTO UserProfile { get; set; } = new();

        [BindProperty]
        public SaveAddressDTO Address { get; set; } = new();

        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user != null)
            {
                UserProfile.FullName = user.FullName;
                UserProfile.Email = user.Email;
                UserProfile.Phone = user.Phone;
            }

            var address = await _userAddressService.GetAddressByUserIdAsync(userId.Value);
            if (address != null)
            {
                Address.UserId = address.UserId;
                Address.ReceiverName = address.ReceiverName;
                Address.ReceiverPhone = address.ReceiverPhone;
                Address.AddressLine = address.AddressLine;
                Address.Ward = address.Ward;
                Address.District = address.District;
                Address.City = address.City;
                Address.IsDefault = address.IsDefault;
            }
            else
            {
                Address.UserId = userId.Value;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            if (string.IsNullOrWhiteSpace(UserProfile.FullName) ||
                string.IsNullOrWhiteSpace(UserProfile.Email) ||
                string.IsNullOrWhiteSpace(UserProfile.Phone))
            {
                ErrorMessage = "Please enter full profile information";
                return await OnGetAsync();
            }

            var result = await _userService.UpdateUserAsync(userId.Value, UserProfile);

            if (!result.Success)
            {
                ErrorMessage = result.Message;
                return await OnGetAsync();
            }

            HttpContext.Session.SetString("FullName", UserProfile.FullName);
            HttpContext.Session.SetString("Email", UserProfile.Email);

            Message = "Profile updated successfully";
            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostSaveAddressAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            Address.UserId = userId.Value;

            if (string.IsNullOrWhiteSpace(Address.ReceiverName) ||
                string.IsNullOrWhiteSpace(Address.ReceiverPhone) ||
                string.IsNullOrWhiteSpace(Address.AddressLine) ||
                string.IsNullOrWhiteSpace(Address.Ward) ||
                string.IsNullOrWhiteSpace(Address.District) ||
                string.IsNullOrWhiteSpace(Address.City))
            {
                ErrorMessage = "Please enter full address information";
                return await OnGetAsync();
            }

            var result = await _userAddressService.SaveAddressAsync(Address);

            if (!result.Success)
            {
                ErrorMessage = result.Message;
                return await OnGetAsync();
            }

            Message = "Address saved successfully";
            return await OnGetAsync();
        }
    }
}
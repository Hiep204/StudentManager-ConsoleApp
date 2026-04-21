using Microsoft.EntityFrameworkCore;
using Webapp.DTO;
using OnlineShopServer.Models;
using OnlineShopServer.Services.Interface;
using Webapp.Models;

namespace OnlineShopServer.Services
{
    public class AuthService : IAuthService
    {
        public readonly OnlineShopDbContext _context;
        public AuthService(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task<LoginResultDTO?> LoginAsync(LoginDTO dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.Email == dto.Email &&
                    u.Password == dto.Password &&
                    u.IsActive);
            if (user == null)
            {
                return null;
            }

            return new LoginResultDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName
            };
        }

        public async Task<string?> RegisterAsync(RegisterDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                return "Full name is required";

            if (string.IsNullOrWhiteSpace(dto.Email))
                return "Email is required";

            if (string.IsNullOrWhiteSpace(dto.Password))
                return "Password is required";

            if (dto.Password != dto.ConfirmPassword)
                return "Confirm password does not match";

            var existedUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existedUser != null)
                return "Email already exists";

           
            var customerRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == "Customer");

            if (customerRole == null)
                return "Customer role not found";

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Password = dto.Password,
                RoleId = customerRole.RoleId,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return null;
        }
    }
}

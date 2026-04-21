using Microsoft.EntityFrameworkCore;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;
using Webapp.Models;

namespace OnlineShopServer.Services
{
    public class UserService : IUserService
    {
        private readonly OnlineShopDbContext _context;
        public UserService(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task<ProfileDTO?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                   .Where(x => x.UserId == userId && x.IsActive)
                   .Select(x => new ProfileDTO
                   {
                       UserId = x.UserId,
                       FullName = x.FullName,
                       Email = x.Email,
                       Phone = x.Phone
                   })
                   .FirstOrDefaultAsync();
        }

        public async Task<(bool Success, string Message)> UpdateUserAsync(int userId, UpdateProfileDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName) ||
                  string.IsNullOrWhiteSpace(dto.Email) ||
                  string.IsNullOrWhiteSpace(dto.Phone))
            {
                return (false, "FullName, Email, Phone are required");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);

            if (user == null)
            {
                return (false, "User not found");
            }

            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == dto.Email && x.UserId != userId);

            if (emailExists)
            {
                return (false, "Email already exists");
            }

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return (true, "Profile updated successfully");
        }
    }
}


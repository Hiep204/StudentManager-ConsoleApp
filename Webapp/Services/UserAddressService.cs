using Microsoft.EntityFrameworkCore;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;
using Webapp.Models;

namespace OnlineShopServer.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly OnlineShopDbContext _context;
        public UserAddressService(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task<UserAddressDTO?> GetAddressByUserIdAsync(int userId)
        {
            return await _context.UserAddresses
                 .Where(x => x.UserId == userId)
                 .OrderByDescending(x => x.IsDefault)
                 .ThenBy(x => x.AddressId)
                 .Select(x => new UserAddressDTO
                 {
                     AddressId = x.AddressId,
                     UserId = x.UserId,
                     ReceiverName = x.ReceiverName,
                     ReceiverPhone = x.ReceiverPhone,
                     AddressLine = x.AddressLine,
                     Ward = x.Ward,
                     District = x.District,
                     City = x.City,
                     IsDefault = x.IsDefault
                 })
                 .FirstOrDefaultAsync();
        }

        public async Task<(bool Success, string Message)> SaveAddressAsync(SaveAddressDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ReceiverName) ||
                 string.IsNullOrWhiteSpace(dto.ReceiverPhone) ||
                 string.IsNullOrWhiteSpace(dto.AddressLine) ||
                 string.IsNullOrWhiteSpace(dto.Ward) ||
                 string.IsNullOrWhiteSpace(dto.District) ||
                 string.IsNullOrWhiteSpace(dto.City))
            {
                return (false, "All address fields are required");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.IsActive);

            if (user == null)
            {
                return (false, "User not found");
            }

            var existingAddress = await _context.UserAddresses
                .FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.IsDefault);

            if (dto.IsDefault)
            {
                var oldAddresses = await _context.UserAddresses
                    .Where(x => x.UserId == dto.UserId)
                    .ToListAsync();

                foreach (var item in oldAddresses)
                {
                    item.IsDefault = false;
                }
            }

            if (existingAddress == null)
            {
                var newAddress = new UserAddress
                {
                    UserId = dto.UserId,
                    ReceiverName = dto.ReceiverName,
                    ReceiverPhone = dto.ReceiverPhone,
                    AddressLine = dto.AddressLine,
                    Ward = dto.Ward,
                    District = dto.District,
                    City = dto.City,
                    IsDefault = dto.IsDefault
                };

                _context.UserAddresses.Add(newAddress);
            }
            else
            {
                existingAddress.ReceiverName = dto.ReceiverName;
                existingAddress.ReceiverPhone = dto.ReceiverPhone;
                existingAddress.AddressLine = dto.AddressLine;
                existingAddress.Ward = dto.Ward;
                existingAddress.District = dto.District;
                existingAddress.City = dto.City;
                existingAddress.IsDefault = dto.IsDefault;
            }

            await _context.SaveChangesAsync();

            return (true, "Address saved successfully");
        }
    }
}


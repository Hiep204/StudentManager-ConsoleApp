using Webapp.DTO;

namespace OnlineShopServer.Services.Interface
{
    public interface IUserAddressService
    {
        Task<UserAddressDTO?> GetAddressByUserIdAsync(int userId);
        Task<(bool Success, string Message)> SaveAddressAsync(SaveAddressDTO dto);
    }
}

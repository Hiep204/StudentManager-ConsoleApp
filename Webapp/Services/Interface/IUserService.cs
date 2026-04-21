using Webapp.DTO;

namespace OnlineShopServer.Services.Interface
{
    public interface IUserService
    {
        Task<ProfileDTO?> GetUserByIdAsync(int userId);
        Task<(bool Success, string Message)> UpdateUserAsync(int userId, UpdateProfileDTO dto);
    }
}

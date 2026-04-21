using Webapp.DTO;

namespace OnlineShopServer.Services.Interface
{
    public interface IAuthService
    {
        Task<LoginResultDTO?> LoginAsync(LoginDTO dto);
        Task<string?> RegisterAsync(RegisterDTO dto);

    }
}

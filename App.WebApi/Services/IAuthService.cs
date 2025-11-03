using App.WebApi.Models.Auth;

namespace App.WebApi.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request);
    }
}

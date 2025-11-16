using App.WebApi.Models.Auth;

namespace App.WebApi.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request);
        Task<LoginResponseDto?> AuthenticateAsync(string nombreUsuario, Guid sesionId);
    }
}

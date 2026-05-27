using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Auth;

namespace WIMS.Application.Interfaces.Services.Auth;

public interface IAuthService
{
    Task<ApiResponse<GenerateTokenResponse>> Login(LoginRequest request);
    Task<ApiResponse<GenerateTokenResponse>> RefreshToken(RefreshTokenRequest request);
    Task<ApiResponse<string>> ChangePassword(ChangePasswordRequest request);
    Task<ApiResponse<string>> Logout(LogoutRequest request);
}

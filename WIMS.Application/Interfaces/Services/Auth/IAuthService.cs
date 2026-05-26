using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Auth;

namespace WIMS.Application.Interfaces.Services.Auth;

public interface IAuthService
{
    public Task<ApiResponse<GenerateTokenResponse>> Login(LoginRequest request);
    public Task<ApiResponse<GenerateTokenResponse>> RefreshToken(RefreshTokenRequest request);
}

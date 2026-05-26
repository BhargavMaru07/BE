using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Auth;
using WIMS.Application.Interfaces.Common;
using WIMS.Application.Interfaces.Repositories;
using WIMS.Application.Interfaces.Services.Auth;
using WIMS.Domain.Entity;
using WIMS.Domain.Enums;

namespace WIMS.Application.Service.Auth;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IInputNormalizer _inputNormalizer;

    public AuthService(IJwtService jwtService, IUserRepository userRepository, IPasswordHasher passwordHasher, IInputNormalizer inputNormalizer)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _inputNormalizer = inputNormalizer;
    }

    public async Task<ApiResponse<GenerateTokenResponse>> Login(LoginRequest request)
    {

        request = _inputNormalizer.NormalizeObject(request);

        User? user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            return ApiResponse<GenerateTokenResponse>.Failure("Given Email is not exist.You need to Register First", null, 404);
        }

        if (IsUserInActive(user))
        {
            return ApiResponse<GenerateTokenResponse>.Failure("Your account is inactive. Please contact Admin.", null, 403);
        }

        if (IsUserLocked(user))
        {
            return ApiResponse<GenerateTokenResponse>.Failure($"Your account is locked due to multiple failed login attempts. Please try again after {user.LockedUntil}.", null, 403);
        }


        bool isValidPassword = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (isValidPassword == false)
        {
            IncreaseFailedLoginAttempts(user);

            if (user.Status == EntityStatus.Locked)
            {
                return ApiResponse<GenerateTokenResponse>.Failure($"Your account is locked due to multiple failed login attempts. Please try again after {user.LockedUntil}.", null, 403);
            }

            return ApiResponse<GenerateTokenResponse>.Failure("Invalid Credentials", null, 400);
        }

        GenerateTokenResponse tokenResponse = _jwtService.generateToken(user);
        user.RefreshToken = tokenResponse.RefreshToken;
        user.RefreshTokenExpiryTime = tokenResponse.RefreshTokenExpiryTime;
        user.LastLoginAt = DateTime.UtcNow;
        user.FailedLoginAttempts = 0;

        await _userRepository.UpdateAsync(user);

        return ApiResponse<GenerateTokenResponse>.Success(tokenResponse,"Login Successfully",200);
    }

    private bool IsUserLocked(User user)
    {
        if (user.Status == EntityStatus.Locked && user.LockedUntil != null)
        {
            if (DateTime.UtcNow < user.LockedUntil)
            {
                return true;
            }
            else
            {
                user.Status = EntityStatus.Active;
                user.FailedLoginAttempts = 0;
                user.LockedUntil = null;
                _userRepository.UpdateAsync(user);
                return false;
            }
        }
        return false;
    }

    private bool IsUserInActive(User user)
    {
        return user.Status == EntityStatus.Inactive;
    }

    private void IncreaseFailedLoginAttempts(User user)
    {
        user.FailedLoginAttempts += 1;

        if (user.FailedLoginAttempts >= 5)
        {
            user.Status = EntityStatus.Locked;
            user.LockedUntil = DateTime.UtcNow.AddMinutes(30);
        }

        _userRepository.UpdateAsync(user);
    }


    public async Task<ApiResponse<GenerateTokenResponse>> RefreshToken(RefreshTokenRequest request)
    {
        var refreshToken = _inputNormalizer.Normalize(request.RefreshToken);

        var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);

        if (user == null || user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return ApiResponse<GenerateTokenResponse>.Failure("Invalid refresh token");
        }

        GenerateTokenResponse tokenResponse = _jwtService.generateToken(user);

        user.RefreshToken = tokenResponse.RefreshToken;
        user.RefreshTokenExpiryTime = tokenResponse.RefreshTokenExpiryTime;

        await _userRepository.UpdateAsync(user);

        return ApiResponse<GenerateTokenResponse>.Success(tokenResponse,"Request successful.",200);
    }
}

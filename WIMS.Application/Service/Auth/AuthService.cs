using System.Security.Cryptography;
using MailKit.Net.Smtp;
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
    private readonly ICodeGeneratorService _code;
    private readonly IEmailService _emailService;

    public AuthService(IJwtService jwtService, IUserRepository userRepository, IPasswordHasher passwordHasher, IInputNormalizer inputNormalizer, ICodeGeneratorService code, IEmailService emailService)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _inputNormalizer = inputNormalizer;
        _code = code;
        _emailService = emailService;
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

            if (user.Status == UserStatus.Locked)
            {
                return ApiResponse<GenerateTokenResponse>.Failure($"Your account is locked due to multiple failed login attempts. Please try again after {user.LockedUntil}.", null, 403);
            }

            return ApiResponse<GenerateTokenResponse>.Failure("Invalid Credentials", null, 400);
        }

        GenerateTokenResponse tokenResponse = _jwtService.generateToken(user);
        user.RefreshTokenHash = _passwordHasher.NormalHash(tokenResponse.RefreshToken);
        user.RefreshTokenExpiryTime = tokenResponse.RefreshTokenExpiryTime;
        user.LastLoginAt = DateTime.UtcNow;
        user.FailedLoginAttempts = 0;

        await _userRepository.UpdateAsync(user);

        return ApiResponse<GenerateTokenResponse>.Success(tokenResponse, "Login Successfully", 200);
    }

    private bool IsUserLocked(User user)
    {
        if (user.Status == UserStatus.Locked && user.LockedUntil != null)
        {
            if (DateTime.UtcNow < user.LockedUntil)
            {
                return true;
            }
            else
            {
                user.Status = UserStatus.Active;
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
        return user.Status == UserStatus.Inactive;
    }

    private void IncreaseFailedLoginAttempts(User user)
    {
        user.FailedLoginAttempts += 1;

        if (user.FailedLoginAttempts >= 5)
        {
            user.Status = UserStatus.Locked;
            user.LockedUntil = DateTime.UtcNow.AddMinutes(30);
        }

        _userRepository.UpdateAsync(user);
    }

    public async Task<ApiResponse<GenerateTokenResponse>> RefreshToken(RefreshTokenRequest request)
    {
        var refreshToken = _inputNormalizer.Normalize(request.RefreshToken);

        var user = await _userRepository.GetUserByRefreshTokenAsync(_passwordHasher.NormalHash(refreshToken));

        if (user == null || user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return ApiResponse<GenerateTokenResponse>.Failure("Invalid refresh token", statusCode: 400);
        }

        GenerateTokenResponse tokenResponse = _jwtService.generateToken(user);

        user.RefreshTokenHash = _passwordHasher.NormalHash(tokenResponse.RefreshToken);
        user.RefreshTokenExpiryTime = tokenResponse.RefreshTokenExpiryTime;

        await _userRepository.UpdateAsync(user);

        return ApiResponse<GenerateTokenResponse>.Success(tokenResponse, "Request successful.", 200);
    }


    public async Task<ApiResponse<string>> ChangePassword(ChangePasswordRequest request)
    {
        request = _inputNormalizer.NormalizeObject(request);

        var user = await _userRepository.GetAsync(x => x.Id == request.UserId);

        if (user == null)
        {
            return ApiResponse<string>.Failure("User not found", null, 404);
        }

        if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
        {
            return ApiResponse<string>.Failure("Current password is incorrect", null, 400);
        }

        if (request.CurrentPassword == request.NewPassword)
        {
            return ApiResponse<string>.Failure("New password cannot be the same as the current password", null, 400);
        }

        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);

        await _userRepository.UpdateAsync(user);

        return ApiResponse<string>.Success("Password changed successfully", "Password changed successfully", 200);
    }


    public async Task<ApiResponse<string>> Logout(LogoutRequest request)
    {
        var refreshToken = _inputNormalizer.Normalize(request.RefreshToken);

        var user = await _userRepository.GetUserByRefreshTokenAsync(_passwordHasher.NormalHash(refreshToken));

        if (user == null)
        {
            return ApiResponse<string>.Failure("Invalid refresh token", null, 400);
        }

        user.RefreshTokenHash = null;
        user.RefreshTokenExpiryTime = null;

        await _userRepository.UpdateAsync(user);

        return ApiResponse<string>.Success("Logout Successfully", "Logout Successfully", 200);
    }

    public async Task<ApiResponse<string>> ForgotPassword(ForgotPasswordRequest request)
    {
        string userEmail = _inputNormalizer.NormalizeEmail(request.Email);

        User? user = await _userRepository.GetByEmailAsync(userEmail);

        if (user == null)
            return ApiResponse<string>.Failure("there is no user with given email.you need to register first", statusCode: 404);

        if (!await _emailService.IsEmailDomainValidAsync(userEmail))
        {
            return ApiResponse<string>.Failure("Invalid email domain. Please check and try again.", statusCode: 400);
        }

        var tokenBytes = new byte[32];
        RandomNumberGenerator.Fill(tokenBytes);

        var token = Convert.ToBase64String(tokenBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");

        await _userRepository.BeginTransactionAsync();

        try
        {
            user.PasswordResetTokenHash = _passwordHasher.NormalHash(token);
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);
            await _userRepository.UpdateAsync(user);

            var resetLink =
                $"http://localhost:4200/reset-password" +
                $"?token={Uri.EscapeDataString(token)}" +
                $"&email={Uri.EscapeDataString(user.Email)}";

            var body = $@"
            <p>Hi <strong>{user.FullName}</strong>,</p>
            <p>You requested a password reset. Click the link below:</p>
            <p>
              <a href=""{resetLink}"" 
                 style=""background:#4f46e5;color:#fff;padding:10px 20px;
                         border-radius:8px;text-decoration:none;font-weight:600;"">
                Reset Password
              </a>
            </p>
            <p>This link expires in <strong>30 minutes</strong>.</p>
            <p>If you did not request this, ignore this email.</p>";


            await _emailService.SendEmailAsync(user.Email, "Reset your password", body);
            await _userRepository.CommitTransactionAsync();
            return ApiResponse<string>.Success($"Resent link has been sent to {user.Email}.", statusCode: 200);
        }
        catch (SmtpCommandException ex) when (ex.StatusCode == SmtpStatusCode.MailboxUnavailable || ex.Message.Contains("No such person"))
        {
            await _userRepository.RollbackTransactionAsync();
            return ApiResponse<string>.Failure("The email address does not exist. Please check and try again.", statusCode: 400);
        }
    }

    public async Task<ApiResponse<string>> ResetPassword(ResetPasswordRequest request)
    {
        request = _inputNormalizer.NormalizeObject(request);

        User? user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            return ApiResponse<string>.Failure("User not found", statusCode: 404);
        }

        if (user.PasswordResetTokenHash == null
            || user.PasswordResetTokenExpiry == null
            || user.PasswordResetTokenExpiry <= DateTime.UtcNow
            || user.PasswordResetTokenHash != _passwordHasher.NormalHash(request.Token))
        {
            return ApiResponse<string>.Failure("Invalid or Expired Reset link", statusCode: 400);
        }

        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);

        user.PasswordResetTokenHash = null;
        user.PasswordResetTokenExpiry = null;
        user.RefreshTokenHash = null;
        user.RefreshTokenExpiryTime = null;

        await _userRepository.UpdateAsync(user);
        return ApiResponse<string>.Success("Password reset successfully. You can now log in.", statusCode: 200);
    }

    public async Task<ApiResponse<bool>> ValidateLink(ValidateLinkRequest request)
    {
        request = _inputNormalizer.NormalizeObject(request);

        User? user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            return ApiResponse<bool>.Failure("User Not Found", statusCode: 404);
        }

        if (user.PasswordResetTokenHash == null
            || user.PasswordResetTokenExpiry == null
            || user.PasswordResetTokenExpiry <= DateTime.UtcNow
            || user.PasswordResetTokenHash != _passwordHasher.NormalHash(request.Token))
        {
            return ApiResponse<bool>.Failure("Link is Expired.Link is only for one time use", statusCode: 400);
        }

        return ApiResponse<bool>.Success(true, "Valid link", 200);
    }
}

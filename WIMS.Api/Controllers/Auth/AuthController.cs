using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Auth;
using WIMS.Application.Interfaces.Services.Auth;

namespace WIMS.Api.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.Login(request);

        if (response.IsSuccess == false)
        {
            if (response.StatusCode == 404)
            {
                return NotFound(response);
            }
            else if (response.StatusCode == 403)
            {
                return StatusCode(StatusCodes.Status403Forbidden, response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        if (response.IsSuccess)
        {
            Response.Cookies.Append("RefreshToken", response.Data!.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(10)
            });
        }
        var data = new LoginResponse
        {
            AccessToken = response.Data!.AccessToken
        };
        return Ok(ApiResponse<LoginResponse>.Success(data, response.Message, response.StatusCode));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {

        var refreshToken = Request.Cookies["RefreshToken"];

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized(ApiResponse<string>.Failure("Refresh token not found", null, 401));
        }

        var request = new RefreshTokenRequest
        {
            RefreshToken = refreshToken
        };

        var result = await _authService.RefreshToken(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        if (result.IsSuccess)
        {
            Response.Cookies.Append("RefreshToken", result.Data!.RefreshToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(10)
            });
        }

        var data = new LoginResponse
        {
            AccessToken = result.Data!.AccessToken
        };

        return Ok(ApiResponse<LoginResponse>.Success(data, result.Message, result.StatusCode));
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["RefreshToken"];

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return NotFound(
                ApiResponse<string>.Failure("Refresh token not found", statusCode: 404));
        }

        var request = new LogoutRequest
        {
            RefreshToken = refreshToken
        };

        var result = await _authService.Logout(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        Response.Cookies.Delete("RefreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        return Ok(ApiResponse<string>.Success(result.Message, statusCode: result.StatusCode));
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId)) return Unauthorized();

        request.UserId = userId;

        var result = await _authService.ChangePassword(request);

        if (!result.IsSuccess)
        {
            if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        return Ok(result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var result = await _authService.ForgotPassword(request);

        if (!result.IsSuccess)
        {
            if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var result = await _authService.ResetPassword(request);

        if (!result.IsSuccess)
        {
            if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        Response.Cookies.Delete("RefreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        return Ok(result);
    }

    [HttpPost("validate-link")]
    public async Task<IActionResult> ValidateLink(ValidateLinkRequest request)
    {
        var result = await _authService.ValidateLink(request);

        if (!result.IsSuccess)
        {
            if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        return Ok(result);
    }

}

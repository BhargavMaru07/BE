using Microsoft.AspNetCore.Mvc;
using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Auth;
using WIMS.Application.Interfaces.Services.Auth;

namespace WIMS.Api.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
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
                Expires = DateTime.UtcNow.AddDays(30)
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
            return Unauthorized(ApiResponse<string>.Failure("Refresh token not found",null,401));
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
                Expires = DateTime.UtcNow.AddDays(30)
            });
        }

        var data = new LoginResponse
        {
            AccessToken = result.Data!.AccessToken
        };

        return Ok(ApiResponse<LoginResponse>.Success(data, result.Message,result.StatusCode));
    }
}

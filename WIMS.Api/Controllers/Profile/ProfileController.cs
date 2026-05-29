using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WIMS.Application.DTOs.Profile;
using WIMS.Application.Interfaces.Services.Profile;

namespace WIMS.Api.Controllers.Profile;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;

    public ProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    private int GetCurrentUserId()
        => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var result = await _userProfileService.GetProfile(GetCurrentUserId());

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
    {
        var result = await _userProfileService.UpdateProfile(GetCurrentUserId(), request);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
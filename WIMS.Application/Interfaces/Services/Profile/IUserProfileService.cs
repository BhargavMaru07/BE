using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Profile;

namespace WIMS.Application.Interfaces.Services.Profile;

public interface IUserProfileService
{
    Task<ApiResponse<UserProfileResponse>> GetProfile(int userId);
    Task<ApiResponse<UserProfileResponse>> UpdateProfile(int userId, UpdateProfileRequest request);
}
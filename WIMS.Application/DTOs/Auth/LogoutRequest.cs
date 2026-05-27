using System.ComponentModel.DataAnnotations;

namespace WIMS.Application.DTOs.Auth;

public class LogoutRequest
{
    [Required(ErrorMessage = "RefreshToken is required.")]
    [MaxLength(500, ErrorMessage = "RefreshToken is nor exceed 500 characters.")]
    public required string RefreshToken { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace WIMS.Application.DTOs.Auth;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "RefreshToken is required.")]
    [MaxLength(500,ErrorMessage = "RefreshToken is nor exceed 500 characters.")]
    public required string RefreshToken {get; set;}
}

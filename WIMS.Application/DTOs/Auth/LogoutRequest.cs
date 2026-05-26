using System.ComponentModel.DataAnnotations;

namespace WIMS.Application.DTOs.Auth;

public class LogoutRequest
{
    [Required]
    public required string RefreshToken { get; set; } 
}

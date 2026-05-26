namespace WIMS.Application.DTOs;

public class GenerateTokenResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
     public DateTime RefreshTokenExpiryTime { get; set; }
}

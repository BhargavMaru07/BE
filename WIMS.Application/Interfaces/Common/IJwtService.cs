using WIMS.Application.DTOs;
using WIMS.Domain.Entity;

namespace WIMS.Application.Interfaces.Common;

public interface IJwtService
{
    GenerateTokenResponse generateToken(User user);
}

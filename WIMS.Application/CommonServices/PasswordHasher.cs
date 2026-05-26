using WIMS.Application.Interfaces.Common;

namespace WIMS.Application.CommonServices;

public class PasswordHasher:IPasswordHasher
{
     public string Hash(string password)
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(password);
        return hash;
    }

    public bool Verify(string password, string hash)
    {
        bool valid = BCrypt.Net.BCrypt.Verify(password, hash);
        return valid;
    }
}

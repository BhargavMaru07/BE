namespace WIMS.Application.Interfaces.Common;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
    string RefreshHash(string token);
}

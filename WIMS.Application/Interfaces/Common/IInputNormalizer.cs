namespace WIMS.Application.Interfaces.Common;

public interface IInputNormalizer
{
    string Normalize(string value);

    string NormalizeEmail(string value);

    T NormalizeObject<T>(T obj);
}
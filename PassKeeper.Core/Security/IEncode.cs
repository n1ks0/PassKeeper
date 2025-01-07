namespace PassKeeper.Core.Security;

public interface IEncode
{
    Task<string> EncodeAsync(string value);
    Task<string> DecodeAsync(string value);
}
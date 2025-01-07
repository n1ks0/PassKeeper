namespace PassKeeper.Core.Security;

public interface IEncode
{
    Task<string> Encode(string value);
    Task<string> Decode(string value);
}
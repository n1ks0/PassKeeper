namespace PassKeeper.Core.Security;

//TODO:реализовать шифрование
public class Encoder : IEncode
{
    public Task<string> EncodeAsync(string value)
    {
        return Task.FromResult(value);
    }

    public Task<string> DecodeAsync(string value)
    {
        return Task.FromResult(value);
    }
}
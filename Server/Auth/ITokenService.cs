public interface ITokenService
{
    string BuildToken(string key, string issuer, UserDto user);

    RefreshToken BuildRefreshToken();

    string Refresh(string key, string issuer, RefreshToken token);
}
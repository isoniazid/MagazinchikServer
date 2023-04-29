public interface ITokenService
{
    string BuildToken(string key, string issuer, UserDto user);

    RefreshToken BuildRefreshToken(User user);

    string Refresh(string key, string issuer, RefreshToken token);
}
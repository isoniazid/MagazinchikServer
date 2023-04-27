public class TokenService : ITokenService
{
    private TimeSpan expiryDuration = Starter.AccessTokenTime;
    public string BuildToken(string key, string issuer, UserDto user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString())
        };

        var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(secureKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(
            issuer,
             issuer,
             claims,
             expires: DateTime.Now.Add(expiryDuration),
             signingCredentials: credentials
             );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public RefreshToken BuildRefreshToken()
    {
        var rTokenVal = new byte[32];
        var rndGen = new System.Random();
        rndGen.NextBytes(rTokenVal);
        return new RefreshToken
        {
            Value = Convert.ToBase64String(rTokenVal),
            Expires = DateTime.UtcNow+Starter.RefreshTokenTime
        };
    }

    public string Refresh(string key, string issuer, RefreshToken token)
    {
        var userDto = new UserDto(token.User.Email, token.User.Id);
        return BuildToken(key, issuer,userDto);
    }
}
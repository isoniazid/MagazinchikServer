public class TokenService : ITokenService
{
    private TimeSpan expiryDuration = new TimeSpan(0, 30, 0);
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
}
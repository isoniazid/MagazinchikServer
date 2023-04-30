public interface IRefreshTokenRepository : IDisposable
{
    Task <List<RefreshToken>> GetTokensAsync();

    Task<RefreshToken> GetTokenAsync(int tokenId);

    Task<RefreshToken> GetTokenAsync(string tokenValue);
    Task InsertTokenAsync(RefreshToken token);

    Task UpdateTokenAsync(RefreshToken token);
    

    Task DeleteTokenAsync(int tokenId);

    Task DeleteTokenAsync(string tokenVal);

    Task SaveAsync();
}
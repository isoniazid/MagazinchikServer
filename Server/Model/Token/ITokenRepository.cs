public interface ITokenRepository : IDisposable
{
    Task <List<Token>> GetTokensAsync();

    Task<Token> GetTokenAsync(int tokenId);

    Task InsertTokenAsync(Token token);

    Task UpdateTokenAsync(Token token);

    Task DeleteTokenAsync(int tokenId);

    Task SaveAsync();
}
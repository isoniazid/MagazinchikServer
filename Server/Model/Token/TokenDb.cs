public class TokenDb: DbContext
{
    public TokenDb(DbContextOptions<TokenDb> options) : base(options)
    {}

    public DbSet<Token> Tokens => Set<Token>();
}
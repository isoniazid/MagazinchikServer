public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteTokenAsync(int tokenId)
    {
        var tokenFromDb = await _context.Tokens.FindAsync(new object[] { tokenId });
        if (tokenFromDb == null) throw new APIException($"No such token with id {tokenId}", StatusCodes.Status404NotFound);
        _context.Tokens.Remove(tokenFromDb);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<RefreshToken> GetTokenAsync(int tokenId)
    {
        var result = await _context.Tokens.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == tokenId);
        //var result = await _context.Tokens.FindAsync(new object[] { tokenId });
        if (result == null) throw new APIException("No such token", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<RefreshToken>> GetTokensAsync()
    {
        return await _context.Tokens.ToListAsync();
    }

    public async Task InsertTokenAsync(RefreshToken token)
    {
        await _context.Tokens.AddAsync(token);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTokenAsync(RefreshToken token)
    {
        var tokenFromDb = await _context.Tokens.FindAsync(new object[] { token.Id });
        if (tokenFromDb == null) throw new APIException("No such token", StatusCodes.Status404NotFound);
        
        foreach(var prop in typeof(RefreshToken).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(tokenFromDb, prop.GetValue(token)); //NB обобщил
        }
    }

    public async Task<RefreshToken> GetTokenAsync(string tokenValue)
    {
         var result = await _context.Tokens.Include(t => t.User).FirstOrDefaultAsync(t => t.Value == tokenValue);
        //var result = await _context.Tokens.FindAsync(new object[] { tokenId });
        if (result == null) throw new APIException("No such token", StatusCodes.Status404NotFound);
        else return result;
    }
}
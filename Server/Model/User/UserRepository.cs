public class UserRepository : IUserRepository
{

    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteUserAsync(int userId)
    {
        var userFromDb = await _context.Users.FindAsync(new object[] { userId });
        if (userFromDb == null) throw new APIException($"No such user with id {userId}", StatusCodes.Status404NotFound);
        _context.Users.Remove(userFromDb);
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

    public async Task<User> GetUserAsync(int userId)
    {
        var result = await _context.Users.FindAsync(new object[] { userId });
        if (result == null) throw new APIException("No such user", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task InsertUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        var userFromDb = await _context.Users.FindAsync(new object[] { user.Id });
        if (userFromDb == null) throw new APIException("No such user", StatusCodes.Status404NotFound);
        userFromDb.Id = user.Id;
        userFromDb.Email = user.Email;
        userFromDb.Name = user.Name;
    }

    public async Task<string> GetUserTokenAsync(int userId)
    {
        var tokenFromDb = await _context.Tokens.Where(token => token.User.Id == userId).FirstOrDefaultAsync();
        if(tokenFromDb == null) throw new APIException("No token for this user", StatusCodes.Status404NotFound);
        return tokenFromDb.RefreshToken;
    }

    public async Task CreateUserTokenAsync(int userId)
    {
        var userFromDb = await _context.Users.FindAsync(new object[] { userId });
        if(userFromDb == null) throw new APIException("Can't create token because user with such id does not exist",StatusCodes.Status424FailedDependency);
        var currentToken = new Token();
        currentToken.User = userFromDb;
        currentToken.RefreshToken = "ЖОПА";
        await _context.Tokens.AddAsync(currentToken);
    }
}
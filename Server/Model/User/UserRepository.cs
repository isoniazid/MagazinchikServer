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
        user.Password = HashPassword(user.Password, user.Email); //NB Вместо соли используется емейл, который уникален
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



    public UserDto GetUser(User user)
    {
        var userFromDb = _context.Users.FirstOrDefault(
         u =>
         string.Equals(u.Email, user.Email) &&
         string.Equals(u.Password, HashPassword(user.Password,user.Email))) ?? throw new APIException("User not found", 404);
        var userDto = new UserDto(userFromDb.Email, userFromDb.Id);
        return userDto;
    }

    public string HashPassword(string password, string email)//Я не стал генерировать соль, потому что емейл у пользователя уже уникальный. Просто суммирую пароль и емейл и смотрю, чтобы все сошлось
    {
        var encrypter = new System.Security.Cryptography.HMACSHA256();
        encrypter.Key = Starter.passwordHashKey;
        return Convert.ToBase64String(encrypter.ComputeHash(Encoding.UTF8.GetBytes(password+email))) ?? throw new Exception("Incorrect values for HashPassword");
    }
}
public interface IUserRepository : IDisposable
{
    Task <List<User>> GetUsersAsync();

    Task<User> GetUserAsync(int userId);

    Task InsertUserAsync(User user);

    Task UpdateUserAsync(User user);

    Task DeleteUserAsync(int userId);

    UserDto GetUser(User user);

    string HashPassword(string password, string email);

    Task SaveAsync();
}
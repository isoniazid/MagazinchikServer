public interface ICartRepository : IDisposable
{
    Task <List<Cart>> GetCartsAsync();

    Task<Cart> GetCartAsync(int cartId);

    Task InsertCartAsync(Cart cart);

    Task UpdateCartAsync(Cart cart);

    Task DeleteCartAsync(int cartId);

    Task SaveAsync();
}
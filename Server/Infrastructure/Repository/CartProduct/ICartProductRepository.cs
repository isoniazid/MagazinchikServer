public interface ICartProductRepository : IDisposable
{
    Task <List<CartProduct>> GetCartProductsAsync();

    Task<CartProduct> GetCartProductAsync(int cartProductId);

    Task InsertCartProductAsync(CartProduct cartProduct);

    Task UpdateCartProductAsync(CartProduct cartProduct);

    Task DeleteCartProductAsync(int cartProductId);

    Task SaveAsync();
}
public interface ICartProductRepository : IDisposable
{
    Task <List<CartProduct>> GetCartProductsAsync();

    Task<CartProduct> GetCartProductAsync(int cartProductId);

    Task InsertCartProductAsync(CartProduct cartProduct);

    Task<CartProduct> UpdateCartProductAsync(CartProductDto dto);

    Task DeleteCartProductAsync(int cartProductId);

    List<ProductDto> DeleteAllCartProductAsync (int cartId);

    Task SaveAsync();
}
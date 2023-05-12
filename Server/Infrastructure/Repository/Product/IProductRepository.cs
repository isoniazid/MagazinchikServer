public interface IProductRepository : IDisposable
{

    //Базовые круды
    Task<List<Product>> GetProductsAsync();

    Task<Product> GetProductAsync(int productId);

    Task<ProductDto> GetProductDtoAsync(int productId);

    Task<ProductDto> GetProductDtoAsync (string slug);

    Task InsertProductAsync(Product product);

    Task UpdateProductAsync(Product product);

    Task DeleteProductAsync(int productId);

    Task SaveAsync();

    // Круды для работы с полями
    /* Task<List<CartProduct>> GetCartsAsync(int productId);
    Task<List<OrderProduct>> GetOrdersAsync(int productId);
    Task<List<Favourite>> GetFavouritesAsync(int productId);
    Task<List<Comment>> GetCommentsAsync(int productId); */
}
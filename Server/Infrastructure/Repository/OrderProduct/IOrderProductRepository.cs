public interface IOrderProductRepository : IDisposable
{

    //Базовые круды
    Task<List<OrderProduct>> GetOrderProductsAsync();

    Task<OrderProduct> GetOrderProductAsync(int orderProductId);

    Task InsertOrderProductAsync(OrderProduct orderProduct);

    Task UpdateOrderProductAsync(OrderProduct orderProduct);

    Task DeleteOrderProductAsync(int orderProductId);

    Task SaveAsync();

    // Круды для работы с полями
    /* Task<List<CartOrderProduct>> GetCartsAsync(int orderProductId);
    Task<List<OrderProduct>> GetOrdersAsync(int orderProductId);
    Task<List<Favourite>> GetFavouritesAsync(int orderProductId);
    Task<List<Comment>> GetCommentsAsync(int orderProductId); */
}
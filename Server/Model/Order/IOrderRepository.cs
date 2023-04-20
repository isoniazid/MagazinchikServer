public interface IOrderRepository : IDisposable
{
    Task <List<Order>> GetOrdersAsync();

    Task<Order> GetOrderAsync(int orderId);

    Task InsertOrderAsync(Order order);

    Task UpdateOrderAsync(Order order);

    Task DeleteOrderAsync(int orderId);

    Task SaveAsync();
}
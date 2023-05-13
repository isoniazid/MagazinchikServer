public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteOrderAsync(int orderId)
    {
        var orderFromDb = await _context.Orders.FindAsync(new object[] { orderId });
        if (orderFromDb == null) throw new APIException($"No such order with id {orderId}", StatusCodes.Status404NotFound);
        _context.Orders.Remove(orderFromDb);
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

    
    public async Task<Order> GetOrderAsync(int orderId)
    {
        var result = await _context.Orders.FindAsync(new object[] { orderId });
        if (result == null) throw new APIException("No such order", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task InsertOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        var orderFromDb = await _context.Orders.FindAsync(new object[] { order.Id });
        if (orderFromDb == null) throw new APIException("No such order", StatusCodes.Status404NotFound);
        
        foreach(var prop in typeof(Order).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(orderFromDb, prop.GetValue(order)); //NB обобщил
        }
    }
}
public class OrderProductRepository : IOrderProductRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public OrderProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteOrderProductAsync(int orderProductId)
    {
        var orderProductFromDb = await _context.OrderProducts.FindAsync(new object[] { orderProductId });
        if (orderProductFromDb == null) throw new APIException($"No such orderProduct with id {orderProductId}", StatusCodes.Status404NotFound);
        _context.OrderProducts.Remove(orderProductFromDb);
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

    public async Task<OrderProduct> GetOrderProductAsync(int orderProductId)
    {
        var result = await _context.OrderProducts.FindAsync(new object[] { orderProductId });
        if (result == null) throw new APIException("No such orderProduct", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<OrderProduct>> GetOrderProductsAsync()
    {
        return await _context.OrderProducts.ToListAsync();
    }

    public async Task InsertOrderProductAsync(OrderProduct orderProduct)
    {
        await _context.OrderProducts.AddAsync(orderProduct);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderProductAsync(OrderProduct orderProduct)
    {
        var orderProductFromDb = await _context.OrderProducts.FindAsync(new object[] { orderProduct.Id });
        if (orderProductFromDb == null) throw new APIException("No such orderProduct", StatusCodes.Status404NotFound);
        
        foreach(var prop in typeof(OrderProduct).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(orderProductFromDb, prop.GetValue(orderProduct)); //NB обобщил
        }
    }
}
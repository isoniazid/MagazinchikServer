public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteCartAsync(int cartId)
    {
        var cartFromDb = await _context.Carts.FindAsync(new object[] { cartId });
        if (cartFromDb == null) throw new APIException($"No such cart with id {cartId}", StatusCodes.Status404NotFound);
        _context.Carts.Remove(cartFromDb);
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

    public async Task<Cart> GetCartAsync(int cartId)
    {
        var result = await _context.Carts.FindAsync(new object[] { cartId });
        if (result == null) throw new APIException("No such cart", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<Cart>> GetCartsAsync()
    {
        return await _context.Carts.ToListAsync();
    }

    public async Task InsertCartAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCartAsync(Cart cart)
    {
        var cartFromDb = await _context.Carts.FindAsync(new object[] { cart.Id });
        if (cartFromDb == null) throw new APIException("No such cart", StatusCodes.Status404NotFound);
        
        foreach(var prop in typeof(Cart).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(cartFromDb, prop.GetValue(cart)); //NB обобщил
        }
    }
}
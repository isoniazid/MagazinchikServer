public class CartProductRepository : ICartProductRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public CartProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteCartProductAsync(int cartProductId)
    {
        var cartProductFromDb = await _context.CartProducts.FindAsync(new object[] { cartProductId });
        if (cartProductFromDb == null) throw new APIException($"No such cartProduct with id {cartProductId}", StatusCodes.Status404NotFound);
        _context.CartProducts.Remove(cartProductFromDb);
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

    public async Task<CartProduct> GetCartProductAsync(int cartProductId)
    {
        var result = await _context.CartProducts.FindAsync(new object[] { cartProductId });
        if (result == null) throw new APIException("No such cartProduct", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<CartProduct>> GetCartProductsAsync()
    {
        return await _context.CartProducts.ToListAsync();
    }

    public async Task InsertCartProductAsync(CartProduct cartProduct)
    {
        await _context.CartProducts.AddAsync(cartProduct);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCartProductAsync(CartProduct cartProduct)
    {
        var cartProductFromDb = await _context.CartProducts.FindAsync(new object[] { cartProduct.Id });
        if (cartProductFromDb == null) throw new APIException("No such cartProduct", StatusCodes.Status404NotFound);
        
        foreach(var prop in typeof(CartProduct).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(cartProductFromDb, prop.GetValue(cartProduct)); //NB обобщил
        }
    }
}
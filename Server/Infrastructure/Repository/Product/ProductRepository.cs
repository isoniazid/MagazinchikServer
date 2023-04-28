public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteProductAsync(int productId)
    {
        var productFromDb = await _context.Products.FindAsync(new object[] { productId });
        if (productFromDb == null) throw new APIException($"No such product with id {productId}", StatusCodes.Status404NotFound);
        _context.Products.Remove(productFromDb);
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

    public async Task<Product> GetProductAsync(int productId)
    {
        var result = await _context.Products.FindAsync(new object[] { productId });
        if (result == null) throw new APIException("No such product", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task InsertProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        var productFromDb = await _context.Products.FindAsync(new object[] { product.Id });
        if (productFromDb == null) throw new APIException("No such product", StatusCodes.Status404NotFound);
        
        foreach(var prop in typeof(Product).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(productFromDb, prop.GetValue(product)); //NB обобщил
        }
    }

    public async Task<Product> GetProductAsync(string slug)
    {
        return await _context.Products.FirstOrDefaultAsync(u => string.Equals(u.Slug, slug)) ?? throw new APIException("No such product", StatusCodes.Status404NotFound);
    }
}


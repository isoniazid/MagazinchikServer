public class ProductPhotoRepository : IProductPhotoRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public ProductPhotoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteProductPhotoAsync(int productPhotoId)
    {
        var productPhotoFromDb = await _context.ProductPhotos.FindAsync(new object[] { productPhotoId });
        if (productPhotoFromDb == null) throw new APIException($"No such productPhoto with id {productPhotoId}", StatusCodes.Status404NotFound);
        _context.ProductPhotos.Remove(productPhotoFromDb);
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

    public async Task<ProductPhoto> GetProductPhotoAsync(int productPhotoId)
    {
        var result = await _context.ProductPhotos.FindAsync(new object[] { productPhotoId });
        if (result == null) throw new APIException("No such productPhoto", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<ProductPhoto>> GetProductPhotosAsync()
    {
        return await _context.ProductPhotos.ToListAsync();
    }

    public async Task InsertProductPhotoAsync(ProductPhoto productPhoto)
    {
        await _context.ProductPhotos.AddAsync(productPhoto);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductPhotoAsync(ProductPhoto productPhoto)
    {
        var productPhotoFromDb = await _context.ProductPhotos.FindAsync(new object[] { productPhoto.Id });
        if (productPhotoFromDb == null) throw new APIException("No such productPhoto", StatusCodes.Status404NotFound);
        
        foreach(var prop in typeof(ProductPhoto).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(productPhotoFromDb, prop.GetValue(productPhoto)); //NB обобщил
        }
    }
}
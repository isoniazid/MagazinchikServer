public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
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

    public async Task DeleteProductAsync(int productId)
    {
        var productFromDb = await _context.Products.FindAsync(new object[] { productId });
        if (productFromDb == null) throw new APIException($"No such product with id {productId}", StatusCodes.Status404NotFound);
        _context.Products.Remove(productFromDb);
    }

    public async Task<ProductDto> GetProductDtoAsync(int productId)
    {
        var productFromDb = await _context.Products.FindAsync(new object[] { productId });
        if (productFromDb == null) throw new APIException("No such product", StatusCodes.Status404NotFound);


        var photosList = _context.ProductPhotos.ToList().Where(p => p.ProductId == productFromDb.Id);
        List<int> photoIds = new List<int>();
        
        if (photosList != null)
        {
            foreach (var photo in productFromDb.Photos)
            {
                photoIds.Add(photo.Id);
            }
        }

        var result = new ProductDto(
            productFromDb.Id,
            productFromDb.Name,
            productFromDb.Slug,
            productFromDb.Price,
            productFromDb.Description,
            productFromDb.CommentsCount,
            productFromDb.AverageRating,
            photoIds.ToArray());

        return result;
    }
    public async Task<List<ProductDto>> GetProductsAsync()
    {
        var result = new List<ProductDto>();
        var raw_list = await _context.Products.Include(p => p.Photos).ToListAsync();

        foreach(var element in raw_list)
        {
            result.Add( new ProductDto(element.Id,
            element.Name,
            element.Slug,
            element.Price,
            element.Description,
            element.CommentsCount,
            element.AverageRating,
            element.Photos.Select(p => p.Id).ToArray()));
        }

        return result;
    }


    public async Task InsertProductAsync(Product product)
    {
        product.CreatedNow();
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

        foreach (var prop in typeof(Product).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(productFromDb, prop.GetValue(product)); //NB обобщил
        }

        productFromDb.Update();
    }

    public async Task<ProductDto> GetProductDtoAsync(string slug)
    {
        var productFromDb = await _context.Products.FirstOrDefaultAsync(u => string.Equals(u.Slug, slug))
        ?? throw new APIException("No such product", StatusCodes.Status404NotFound);


        var photosList = _context.ProductPhotos.ToList().Where(p => p.ProductId == productFromDb.Id);
        List<int> photoIds = new List<int>();
        if (photosList != null)
        {
            foreach (var photo in productFromDb.Photos)
            {
                photoIds.Add(photo.Id);
            }
        }

        var result = new ProductDto(
            productFromDb.Id,
            productFromDb.Name,
            productFromDb.Slug,
            productFromDb.Price,
            productFromDb.Description,
            productFromDb.CommentsCount,
            productFromDb.AverageRating,
            photoIds.ToArray());

        return result;
    }

    public async Task<Product> GetProductAsync(int productId)
    {
        var productFromDb = await _context.Products.FindAsync(new object[] { productId })
        ?? throw new APIException("No such product", StatusCodes.Status404NotFound);

        return productFromDb;

    }

    public  List<Product> GetAllProductsByCartId(int cartId)
    {
        var productList = new List<Product>();
        var cartProducts =  _context.CartProducts.Include(p => p.Product).ToList().Where(u => u.CartId == cartId);
        foreach (var cpr in cartProducts)
        {
            productList.Add(cpr.Product ?? throw new APIException("trying to add null product",400));
        }
        return productList;
    }
}


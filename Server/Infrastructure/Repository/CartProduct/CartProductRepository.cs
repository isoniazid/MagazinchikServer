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
        var exists = await _context.CartProducts.FirstOrDefaultAsync(p => p.ProductId == cartProduct.ProductId && p.CartId == cartProduct.CartId);
        if (exists == null)
        {
            cartProduct.CreatedNow();
            await _context.CartProducts.AddAsync(cartProduct);
        }
        else throw new APIException("position for such product already exists in cart. Use PATCH instead", 400);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<CartProduct> UpdateCartProductAsync(CartProductDto dto)
    {
        var cartProductFromDb = await _context.CartProducts.FirstOrDefaultAsync(p => p.CartId == dto.cartId && p.ProductId == dto.productId);
        if (cartProductFromDb == null) throw new APIException("No such cartProduct", StatusCodes.Status404NotFound);

        cartProductFromDb.Update();
        cartProductFromDb.ProductCount = dto.productCount;
        return cartProductFromDb;
    }

    public List<ProductDto> DeleteAllCartProductAsync(int cartId)
    {
        var cartProductsToDelete = _context.CartProducts.Include(p => p.Product).ToList().Where(p => p.CartId == cartId)
        ?? throw new APIException("No such cart", 400);

        var rawProductList = cartProductsToDelete.Select(p => p.Product)
        ?? throw new APIException("The cart is empty", 400);
        var result = new List<ProductDto>();

        foreach (Product? element in rawProductList)
        {
            if (element != null)
            {
                result.Add(new ProductDto(element.Id,
                element.Name,
                element.Slug,
                element.Price,
                element.Description,
                element.CommentsCount,
                element.AverageRating,
                element.Photos.Select(p => p.Id).ToArray()));
            }
        }


        _context.CartProducts.RemoveRange(cartProductsToDelete);
        return result;

    }
}
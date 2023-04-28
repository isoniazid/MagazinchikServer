public class FavouriteRepository : IFavouriteRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public FavouriteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteFavouriteAsync(int favouriteId)
    {
        var favouriteFromDb = await _context.Favourites.FindAsync(new object[] { favouriteId });
        if (favouriteFromDb == null) throw new APIException($"No such favourite with id {favouriteId}", StatusCodes.Status404NotFound);
        _context.Favourites.Remove(favouriteFromDb);
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

    public async Task<Favourite> GetFavouriteAsync(int favouriteId)
    {
        var result = await _context.Favourites.FindAsync(new object[] { favouriteId });
        if (result == null) throw new APIException("No such favourite", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<Favourite>> GetFavouritesAsync()
    {
        return await _context.Favourites.ToListAsync();
    }

    public async Task InsertFavouriteAsync(Favourite favourite)
    {
        await _context.Favourites.AddAsync(favourite);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFavouriteAsync(Favourite favourite)
    {
        var favouriteFromDb = await _context.Favourites.FindAsync(new object[] { favourite.Id });
        if (favouriteFromDb == null) throw new APIException("No such favourite", StatusCodes.Status404NotFound);
        
        foreach(var prop in typeof(Favourite).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(favouriteFromDb, prop.GetValue(favourite)); //NB обобщил
        }
    }
}
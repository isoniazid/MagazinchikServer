public class FavouriteDb: DbContext
{
    public FavouriteDb(DbContextOptions<FavouriteDb> options) : base(options)
    {}

    public DbSet<Favourite> Favourites => Set<Favourite>();
}
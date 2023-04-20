public interface IFavouriteRepository : IDisposable
{
    Task <List<Favourite>> GetFavouritesAsync();

    Task<Favourite> GetFavouriteAsync(int favouriteId);

    Task InsertFavouriteAsync(Favourite favourite);

    Task UpdateFavouriteAsync(Favourite favourite);

    Task DeleteFavouriteAsync(int favouriteId);

    Task SaveAsync();
}
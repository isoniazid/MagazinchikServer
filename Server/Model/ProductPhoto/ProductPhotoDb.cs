public class ProductPhotoDb: DbContext
{
    public ProductPhotoDb(DbContextOptions<ProductPhotoDb> options) : base(options)
    {}

    public DbSet<ProductPhoto> ProductPhotos => Set<ProductPhoto>();
}
public class CartProductDb: DbContext
{
    public CartProductDb(DbContextOptions<CartProductDb> options) : base(options)
    {}

    public DbSet<CartProduct> CartProducts => Set<CartProduct>();
}
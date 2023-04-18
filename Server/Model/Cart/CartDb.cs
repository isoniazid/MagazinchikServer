public class CartDb: DbContext
{
    public CartDb(DbContextOptions<CartDb> options) : base(options)
    {}

    public DbSet<Cart> Carts => Set<Cart>();
}
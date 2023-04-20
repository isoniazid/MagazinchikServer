public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    public DbSet<Activation> Activations => Set<Activation>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartProduct> CartProducts => Set<CartProduct>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Favourite> Favourites => Set<Favourite>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductPhoto> ProductPhotos => Set<ProductPhoto>();
    public DbSet<Rate> Rates => Set<Rate>();
    public DbSet<Token> Tokens => Set<Token>();
    public DbSet<User> Users => Set<User>();


}
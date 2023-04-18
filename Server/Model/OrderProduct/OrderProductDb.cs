public class OrderProductDb: DbContext
{
    public OrderProductDb(DbContextOptions<OrderProductDb> options) : base(options)
    {}

    public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();
}
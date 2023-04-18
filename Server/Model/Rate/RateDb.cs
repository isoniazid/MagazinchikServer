public class RateDb: DbContext
{
    public RateDb(DbContextOptions<RateDb> options) : base(options)
    {}

    public DbSet<Rate> Rates => Set<Rate>();
}
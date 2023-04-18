public class ActivationDb : DbContext
{
    public ActivationDb(DbContextOptions<ActivationDb> options) : base(options)
    { }

    public DbSet<Activation> Activations => Set<Activation>();
}
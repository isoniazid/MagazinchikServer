public class CommentDb: DbContext
{
    public CommentDb(DbContextOptions<CommentDb> options) : base(options)
    {}

    public DbSet<Comment> Comments => Set<Comment>();
}
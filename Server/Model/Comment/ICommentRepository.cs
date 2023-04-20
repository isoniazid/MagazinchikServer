public interface ICommentRepository : IDisposable
{
    Task <List<Comment>> GetCommentsAsync();

    Task<Comment> GetCommentAsync(int commentId);

    Task InsertCommentAsync(Comment comment);

    Task UpdateCommentAsync(Comment comment);

    Task DeleteCommentAsync(int commentId);

    Task SaveAsync();
}
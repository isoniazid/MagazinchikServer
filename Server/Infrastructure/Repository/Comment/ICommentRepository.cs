public interface ICommentRepository : IDisposable
{
    Task <List<Comment>> GetCommentsAsync();

    List<CommentDto> GetAllCommentDtoForProduct(int id);

    List<CommentDto> GetAllCommentDtoForUser(int id);

    Task<Comment> GetCommentAsync(int commentId);

    Task InsertCommentAsync(Comment comment);

    Task UpdateCommentAsync(Comment comment);

    Task DeleteCommentAsync(int commentId);

    Task SaveAsync();
}
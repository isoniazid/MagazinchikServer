public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteCommentAsync(int commentId)
    {
        var commentFromDb = await _context.Comments.FindAsync(new object[] { commentId });
        if (commentFromDb == null) throw new APIException($"No such comment with id {commentId}", StatusCodes.Status404NotFound);
        _context.Comments.Remove(commentFromDb);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<Comment> GetCommentAsync(int commentId)
    {
        var result = await _context.Comments.FindAsync(new object[] { commentId });
        if (result == null) throw new APIException("No such comment", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<Comment>> GetCommentsAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task InsertCommentAsync(Comment comment)
    {
        comment.CreatedNow();
        await _context.Comments.AddAsync(comment);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        var commentFromDb = await _context.Comments.FindAsync(new object[] { comment.Id });
        if (commentFromDb == null) throw new APIException("No such comment", StatusCodes.Status404NotFound);
        
        foreach(var prop in typeof(Comment).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(commentFromDb, prop.GetValue(comment)); //NB обобщил
        }

        commentFromDb.Update();
    }

    public List<CommentDto> GetAllCommentDtoForProduct(int id)
    {
        var DtoList = new List<CommentDto>();
        //NB
        var commentList = _context.Comments.Include(p => p.User).Include(p => p.Product).ToList().Where(p => p.ProductId == id);


        foreach(var comment in commentList)
        {
            DtoList.Add(new CommentDto(comment.Id,comment.User.Id,comment.User.Name,comment.Product.Name,comment.Product.Slug,comment.Text,comment.CreatedAt, comment.UpdatedAt, -100));
        }

        return DtoList;

    }

    public List<CommentDto> GetAllCommentDtoForUser(int id)
    {
        var DtoList = new List<CommentDto>();
        //NB
        var commentList = _context.Comments.Include(p => p.User).Include(p => p.Product).ToList().Where(p => p.UserId == id);


        foreach(var comment in commentList)
        {
            DtoList.Add(new CommentDto(comment.Id,comment.User.Id,comment.User.Name,comment.Product.Name,comment.Product.Slug,comment.Text,comment.CreatedAt, comment.UpdatedAt, -100));
        }

        return DtoList;
    }
}
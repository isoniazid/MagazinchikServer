public class CommentAPI
{
    public void Register(WebApplication app)
    {
        //Получить всех comment
        app.MapGet("/api/comment", async (ICommentRepository repo) => Results.Ok(await repo.GetCommentsAsync()))
        .Produces<List<Comment>>(StatusCodes.Status200OK)
        .WithName("GetAllComment")
        .WithTags("comment");

        //Получить comment по айди
        app.MapGet("/api/comment/{id}", async (int id, ICommentRepository repo) => await repo.GetCommentAsync(id) is Comment comment
        ? Results.Ok(comment)
        : Results.NotFound())
        .Produces<Comment>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetComment by id")
        .WithTags("comment");

        //Добавить comment с параметрами
        app.MapPost("/api/comment", async ([FromBody] Comment comment, ICommentRepository repo) =>
        {
            await repo.InsertCommentAsync(comment);
            await repo.SaveAsync();
            return Results.Created($"/api/{comment.Id}", comment);
        })
        .Accepts<Comment>("application/json")
        .Produces<Comment>(StatusCodes.Status201Created)
        .WithName("Adds comment by params")
        .WithTags("comment");

        //Изменить comment
        app.MapPut("/api/comment", async ([FromBody] Comment comment, ICommentRepository repo) =>
        {
            await repo.UpdateCommentAsync(comment);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<Comment>("application/json")
        .WithName("Replaces comment with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("comment");

        //Удалить юзера
        app.MapDelete("/api/comment/{id}", async (int id, ICommentRepository repo) =>
        {
            await repo.DeleteCommentAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteComment")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("comment");
    }
}
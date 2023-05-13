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


        //Получить comment по айди товара
        app.MapGet("/api/comment/product/{id}", (int id, ICommentRepository repo) => 
        {
            return Results.Ok(repo.GetAllCommentDtoForProduct(id));
        })
        .Produces<List<CommentDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetComment by id of good")
        .WithTags("comment");

        //Получить comment по айди пользователя
        app.MapGet("/api/comment/user/{id}",  (int id, ICommentRepository repo) => 
        {
            return Results.Ok(repo.GetAllCommentDtoForUser(id));
        })
        .Produces<List<CommentDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetComment by id of user")
        .WithTags("comment");

        //Добавить comment
        app.MapPost("/api/comment", [Authorize] async ([FromBody] CommentToSendDto sendDto,
         ICommentRepository repo, IProductRepository prodRepo, IUserRepository userRepo, HttpContext context) =>
        {

            int jwtId = Convert.ToInt32(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new APIException("Broken acces token", 401));
            if(jwtId != sendDto.userId) throw new APIException("UserId in DTO is not equal to one in access token",StatusCodes.Status401Unauthorized);


            Product commentProduct = await prodRepo.GetProductAsync(sendDto.productId);
            User commentUser = await userRepo.GetUserAsync(sendDto.userId);


            var comment = new Comment()
            {
                Text = sendDto.text,
                User = commentUser,
                Product = commentProduct,
                ProductId = commentProduct.Id,
                UserId = commentUser.Id
            };


            await repo.InsertCommentAsync(comment);
            await repo.SaveAsync();
            return Results.Created($"/api/{comment.Id}",
            new CommentDto(
                comment.Id,
                commentUser.Id,
                commentUser.Name,
                commentProduct.Name,
                commentProduct.Slug,
                comment.Text,
                comment.CreatedAt,
                comment.UpdatedAt));
        })
        .Accepts<CommentToSendDto>("application/json")
        .Produces<CommentDto>(StatusCodes.Status201Created)
        .WithName("Adds comment by params")
        .WithTags("comment");

        //Изменить comment
        app.MapPatch("/api/comment/{id}", [Authorize] async (int id, [FromBody] CommentToSendDto sendDto, ICommentRepository repo, IUserRepository userRepo,
        IProductRepository productRepo, HttpContext context) =>
        {
            int jwtId = Convert.ToInt32(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new APIException("Broken acces token", 401));
            if(jwtId != sendDto.userId) throw new APIException("UserId in DTO is not equal to one in access token",StatusCodes.Status401Unauthorized);

            var userFromDb = await userRepo.GetUserAsync(sendDto.userId);
            var productFromDb = await productRepo.GetProductAsync(sendDto.productId);
            var commentFromDb = await repo.GetCommentAsync(id);
            commentFromDb.Text = sendDto.text;

            await repo.UpdateCommentAsync(commentFromDb);
            await repo.SaveAsync();

            return Results.Ok(new CommentDto(
                id,
                userFromDb.Id,
                userFromDb.Name,
                productFromDb.Name,
                productFromDb.Slug,
                commentFromDb.Text,
                commentFromDb.CreatedAt,
                commentFromDb.UpdatedAt));
        })
        .Accepts<CommentToSendDto>("application/json")
        .WithName("Replaces comment with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("comment");

        //Удалить comment
        app.MapDelete("/api/comment/{id}", async (int id, [FromBody] int userId, ICommentRepository repo, IProductRepository productRepo, 
        IUserRepository userRepo, HttpContext context) =>
        {
            int jwtId = Convert.ToInt32(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new APIException("Broken acces token", 401));
            if(jwtId != userId) throw new APIException("UserId in DTO is not equal to one in access token",StatusCodes.Status401Unauthorized);

            var CommentToDelete = await repo.GetCommentAsync(id);
            CommentToDelete.Product = await productRepo.GetProductAsync(CommentToDelete.ProductId);
            CommentToDelete.User = await userRepo.GetUserAsync(userId);
            await repo.DeleteCommentAsync(id);
            await repo.SaveAsync();
            return Results.Ok(new CommentDto(
                CommentToDelete.Id,
                CommentToDelete.UserId,
                CommentToDelete.User.Name,
                CommentToDelete.Product.Name,
                CommentToDelete.Product.Slug,
                CommentToDelete.Text,
                CommentToDelete.CreatedAt,
                CommentToDelete.UpdatedAt
            ));
        })
        .WithName("DeleteComment")
        .Produces<CommentDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("comment");
    }
}
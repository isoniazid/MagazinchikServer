public class TokenAPI
{
    public void Register(WebApplication app)
    {
        //Получить всех token
        app.MapGet("/api/token", async (ITokenRepository repo) => Results.Ok(await repo.GetTokensAsync()))
        .Produces<List<Token>>(StatusCodes.Status200OK)
        .WithName("GetAllToken")
        .WithTags("token");

        //Получить token по айди
        app.MapGet("/api/token/{id}", async (int id, ITokenRepository repo) => await repo.GetTokenAsync(id) is Token token
        ? Results.Ok(token)
        : Results.NotFound())
        .Produces<Token>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetToken by id")
        .WithTags("token");

        //Добавить token с параметрами
        app.MapPost("/api/token", async ([FromBody] Token token, ITokenRepository repo) =>
        {
            await repo.InsertTokenAsync(token);
            await repo.SaveAsync();
            return Results.Created($"/api/{token.Id}", token);
        })
        .Accepts<Token>("application/json")
        .Produces<Token>(StatusCodes.Status201Created)
        .WithName("Adds token by params")
        .WithTags("token");

        //Изменить token
        app.MapPut("/api/token", async ([FromBody] Token token, ITokenRepository repo) =>
        {
            await repo.UpdateTokenAsync(token);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<Token>("application/json")
        .WithName("Replaces token with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("token");

        //Удалить юзера
        app.MapDelete("/api/token/{id}", async (int id, ITokenRepository repo) =>
        {
            await repo.DeleteTokenAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteToken")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("token");
    }
}
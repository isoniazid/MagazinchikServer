public class UserAPI
{
    public void Register(WebApplication app)
    {
        app.MapGet("/api/user", async (IUserRepository repo) => Results.Ok(await repo.GetUsersAsync()))
        .Produces<List<User>>(StatusCodes.Status200OK)
        .WithName("GetAllUser")
        .WithTags("user");

        app.MapGet("/api/user/{id}", async (int id, IUserRepository repo) => await repo.GetUserAsync(id) is User user
        ? Results.Ok(user)
        : Results.NotFound())
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetUser by id")
        .WithTags("user");

        app.MapPost("/api/user", async ([FromBody] User user, IUserRepository repo) =>
        {
            await repo.InsertUserAsync(user);
            await repo.SaveAsync();
            return Results.Created($"/api/{user.Id}", user);
        })
        .Accepts<User>("application/json")
        .Produces<User>(StatusCodes.Status201Created)
        .WithName("Adds user by params")
        .WithTags("user");

        app.MapPut("/api/user", async ([FromBody] User user, IUserRepository repo) =>
        {
            await repo.UpdateUserAsync(user);
            await repo.SaveAsync();
            return Results.NoContent();
        })
        .Accepts<User>("application/json")
        .WithName("Replaces user with params")
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("user");

        app.MapDelete("api/user/{id}", async (int id, IUserRepository repo) =>
        {
            await repo.DeleteUserAsync(id);
            await repo.SaveAsync();
            return Results.NoContent();
        })
        .WithName("DeleteUser")
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("user");
    }
}
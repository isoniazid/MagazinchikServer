public class UserAPI
{
    public void Register(WebApplication app)
    {
        //Получить всех юзеров
        app.MapGet("/api/user", [Authorize] async (IUserRepository repo) => Results.Ok(await repo.GetUsersAsync()))
        .Produces<List<User>>(StatusCodes.Status200OK)
        .WithName("GetAllUser")
        .WithTags("user");

        //Получить юзера по айди
        app.MapGet("/api/user/{id}", async (int id, IUserRepository repo) => await repo.GetUserAsync(id) is User user
        ? Results.Ok(user)
        : Results.NotFound())
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetUser by id")
        .WithTags("user");

        //Добавить юзера с параметрами
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

        //Изменить юзера
        app.MapPut("/api/user", async ([FromBody] User user, IUserRepository repo) =>
        {
            await repo.UpdateUserAsync(user);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<User>("application/json")
        .WithName("Replaces user with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("user");

        //Удалить юзера
        app.MapDelete("/api/user/{id}", async (int id, IUserRepository repo) =>
        {
            await repo.DeleteUserAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteUser")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("user");

        app.MapPost("api/user/login", [AllowAnonymous] /*async*/ ([FromBody] UserAuthDto inputUser, ITokenService tokenService, IUserRepository repo) =>
        {
            User user = new()
            {
                Email = inputUser.email,
                Password = inputUser.password
            };
            var userDto = repo.GetUser(user);
            if (userDto == null) return Results.Unauthorized();
            var token = tokenService.BuildToken(app.Configuration["Jwt:Key"],
            app.Configuration["Jwt:Issuer"], userDto);
            return Results.Ok(token);
        }).WithName("Login")
        .Accepts<UserAuthDto>("application/json")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithTags("user");
    }
}
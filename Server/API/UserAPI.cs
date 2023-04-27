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
    
        //Обновить аксесс токен
        app.MapPost("api/user/refresh", [AllowAnonymous] async ([FromBody] string refreshToken, ITokenService tokenService, IUserRepository repo, IRefreshTokenRepository tokenRepo) =>
        {
            var currentRefreshToken = await tokenRepo.GetTokenAsync(refreshToken);
            if (currentRefreshToken.Expires < DateTime.UtcNow) throw new APIException($"RefreshToken протух. Он был валиден до даты: {currentRefreshToken.Expires}", StatusCodes.Status401Unauthorized);

            var newAccessToken = tokenService.Refresh(app.Configuration["Jwt:Key"] ?? throw new Exception("JWT:Key mustn't be null!"),
            app.Configuration["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer mustn't be null!"), currentRefreshToken);
            return Results.Ok(newAccessToken);
        }).WithName("Refresh token")
        .Accepts<string>("application/json")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithTags("user");

        //Залогиниться и получить два токена
        app.MapPost("api/user/login", [AllowAnonymous] async ([FromBody] UserAuthDto inputUser, ITokenService tokenService, IUserRepository repo, IRefreshTokenRepository tokenRepo) =>
        {
            User user = new()
            {
                Email = inputUser.email,
                Password = inputUser.password
            };
            var userDto = repo.GetUser(user);
            if (userDto == null) return Results.Unauthorized();
            var token = tokenService.BuildToken(app.Configuration["Jwt:Key"] ?? throw new Exception("JWT:Key mustn't be null!"),
            app.Configuration["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer mustn't be null!"), userDto);

            var refreshToken = tokenService.BuildRefreshToken();

            refreshToken.User = await repo.GetUserAsync(userDto.id) ?? throw new Exception($"User with id {userDto.id} no found");
            await tokenRepo.InsertTokenAsync(refreshToken);
            await tokenRepo.SaveAsync();


            return Results.Ok(new UserTokensDto(token, refreshToken));
        }).WithName("Login")
        .Accepts<UserAuthDto>("application/json")
        .Produces<UserTokensDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithTags("user");
    }
}
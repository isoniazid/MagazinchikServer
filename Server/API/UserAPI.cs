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
        app.MapPost("/api/user/register", async ([FromBody] User user, IUserRepository repo) =>
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
        app.MapGet("api/user/refresh", [AllowAnonymous] async (HttpContext httpContext, ITokenService tokenService, IUserRepository repo, IRefreshTokenRepository tokenRepo) =>
        {
            
            
            string refreshTokenFromCookies = httpContext.Request.Cookies["refresh_token"] ?? throw new APIException("Отсутствует RefreshToken в cookie", StatusCodes.Status401Unauthorized);

            var currentRefreshToken = await tokenRepo.GetTokenAsync(refreshTokenFromCookies);
            
            //Если токен сгорел
            if (currentRefreshToken.Expires < DateTime.UtcNow) throw new APIException($"RefreshToken протух. Он был валиден до даты: {currentRefreshToken.Expires}", StatusCodes.Status401Unauthorized);

            //Если токен скоро сгорит...
            if(currentRefreshToken.Expires-Starter.RefreshTokenThreshold < DateTime.UtcNow) 
            {
                currentRefreshToken = tokenService.BuildRefreshToken(currentRefreshToken.User);
                await tokenRepo.InsertTokenAsync(currentRefreshToken);
                await tokenRepo.SaveAsync();
                SaveToCookies(httpContext, currentRefreshToken);
            }

            var newAccessToken = tokenService.Refresh(app.Configuration["Jwt:Key"] ?? throw new Exception("JWT:Key mustn't be null!"),
            app.Configuration["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer mustn't be null!"), currentRefreshToken);
            return Results.Ok(new UserRefreshDto(currentRefreshToken.User, newAccessToken));
        }).WithName("Refresh token")
        .Produces<UserRefreshDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithTags("user");

        //Залогиниться и получить два токена
        app.MapPost("api/user/login", [AllowAnonymous] async (HttpContext httpContext,[FromBody] UserAuthDto inputUser, ITokenService tokenService, IUserRepository repo, IRefreshTokenRepository tokenRepo) =>
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

            var userFromDb = await repo.GetUserAsync(userDto.id) ?? throw new Exception($"User with id {userDto.id} not found");

            var refreshToken = tokenService.BuildRefreshToken(userFromDb);
            await tokenRepo.InsertTokenAsync(refreshToken);
            await tokenRepo.SaveAsync();

            SaveToCookies(httpContext,refreshToken);


            return Results.Ok(new UserTokensDto(token, refreshToken));
        }).WithName("Login")
        .Accepts<UserAuthDto>("application/json")
        .Produces<UserTokensDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithTags("user");
    }


    private void SaveToCookies(HttpContext context, RefreshToken token)
    {
        if(context.Request.Cookies.ContainsKey("refresh_token"))
        {
            context.Response.Cookies.Delete("refresh_token");
        }

            context.Response.Cookies.Append("refresh_token",token.Value);
    }
}
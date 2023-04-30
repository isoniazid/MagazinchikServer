public class AuthAPI
{
    public void Register(WebApplication app)
    {
        //Добавить юзера с параметрами
        app.MapPost("/api/auth/register", async (HttpContext httpContext, [FromBody] UserRegistrationDto userRegDto, IUserRepository repo, ITokenService tokenService, IRefreshTokenRepository tokenRepo) =>
        {
            var user = new User() { Email = userRegDto.email, Password = userRegDto.password, Name = userRegDto.name };

            await repo.InsertUserAsync(user);
            await repo.SaveAsync();

            var userDto = new UserDto(user.Email, user.Id);

            var token = tokenService.BuildToken(app.Configuration["Jwt:Key"] ?? throw new Exception("JWT:Key mustn't be null!"),
            app.Configuration["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer mustn't be null!"), userDto);

            var refreshToken = tokenService.BuildRefreshToken(user);
            await tokenRepo.InsertTokenAsync(refreshToken);
            await tokenRepo.SaveAsync();

            SaveToCookies(httpContext, refreshToken);

            return Results.Ok(new UserRefreshDto(new userNoDateDto(user.Id, user.Email, user.Name), token));
        })
        .Accepts<UserRegistrationDto>("application/json")
        .Produces<UserRefreshDto>(StatusCodes.Status201Created)
        .WithName("Adds user by params")
        .WithTags("Auth");


        //Выход из профиля с удалением рефреш токена из БД и куки
        app.MapGet("/api/auth/logout", async (HttpContext httpContext, IRefreshTokenRepository tokenRepo) =>
       {
           string refreshTokenFromCookies = httpContext.Request.Cookies["refresh_token"] 
           ?? throw new APIException("Невозможно удалить RefreshToken, т.к. он отсутствует в cookie", StatusCodes.Status404NotFound);


           await tokenRepo.DeleteTokenAsync(refreshTokenFromCookies);
           await tokenRepo.SaveAsync();
           return Results.Ok();
       }).WithName("Logout")
       .Produces(StatusCodes.Status200OK)
       .Produces(StatusCodes.Status401Unauthorized)
       .WithTags("Auth");


        //Обновить аксесс токен
        app.MapGet("/api/auth/refresh", [AllowAnonymous] async (HttpContext httpContext, ITokenService tokenService, IUserRepository repo, IRefreshTokenRepository tokenRepo) =>
        {


            string refreshTokenFromCookies = httpContext.Request.Cookies["refresh_token"] ?? throw new APIException("Отсутствует RefreshToken в cookie", StatusCodes.Status401Unauthorized);

            var currentRefreshToken = await tokenRepo.GetTokenAsync(refreshTokenFromCookies);

            //Если токен сгорел
            if (currentRefreshToken.Expires < DateTime.UtcNow) throw new APIException($"RefreshToken протух. Он был валиден до даты: {currentRefreshToken.Expires}", StatusCodes.Status401Unauthorized);

            //Если токен скоро сгорит...
            if (currentRefreshToken.Expires - Starter.RefreshTokenThreshold < DateTime.UtcNow)
            {
                currentRefreshToken = tokenService.BuildRefreshToken(currentRefreshToken.User);
                await tokenRepo.InsertTokenAsync(currentRefreshToken);
                await tokenRepo.SaveAsync();
                SaveToCookies(httpContext, currentRefreshToken);
            }

            var newAccessToken = tokenService.Refresh(app.Configuration["Jwt:Key"] ?? throw new Exception("JWT:Key mustn't be null!"),
            app.Configuration["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer mustn't be null!"), currentRefreshToken);
            return Results.Ok(new UserRefreshDto(new userNoDateDto(currentRefreshToken.User.Id, currentRefreshToken.User.Email, currentRefreshToken.User.Name), newAccessToken));
        }).WithName("Refresh token")
        .Produces<UserRefreshDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithTags("Auth");

        //Залогиниться и получить два токена
        app.MapPost("/api/auth/login", [AllowAnonymous] async (HttpContext httpContext, [FromBody] UserAuthDto inputUser, ITokenService tokenService, IUserRepository repo, IRefreshTokenRepository tokenRepo) =>
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

            SaveToCookies(httpContext, refreshToken);


            return Results.Ok(new UserRefreshDto(new userNoDateDto(userFromDb.Id, userFromDb.Email, userFromDb.Name), token));
        }).WithName("Login")
        .Accepts<UserAuthDto>("application/json")
        .Produces<UserRefreshDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithTags("Auth");
    }

    private void SaveToCookies(HttpContext context, RefreshToken token)
    {
        if (context.Request.Cookies.ContainsKey("refresh_token"))
        {
            context.Response.Cookies.Delete("refresh_token");
        }

        context.Response.Cookies.Append("refresh_token", token.Value, new CookieOptions() { Secure = true, HttpOnly = true, MaxAge = new TimeSpan(45, 0, 0, 0) });
    }

}
public static class Starter
{

    public static readonly byte[] passwordHashKey = Encoding.UTF8.GetBytes("Здарова Вовчик, а ты че исходники смотришь мои?");

    public static readonly byte[] RefreshHashKey = Encoding.UTF8.GetBytes("Это ключ для хеширования рефреш токенов. Кста Вовыч здарова как жизнь?");
    public static readonly TimeSpan AccessTokenTime = new TimeSpan(0,30,0);
    public static readonly TimeSpan RefreshTokenTime = new TimeSpan(30,0,0,0);

    public static readonly TimeSpan RefreshTokenThreshold = new TimeSpan(5,0,0,0);

    public static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
        });

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<IRateRepository, RateRepository>();
        builder.Services.AddScoped<IProductPhotoRepository, ProductPhotoRepository>();
        builder.Services.AddScoped<IOrderProductRepository, OrderProductRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<ICartProductRepository, CartProductRepository>();
        builder.Services.AddScoped<ICartRepository, CartRepository>();
        builder.Services.AddScoped<IActivationRepository, ActivationRepository>();
        
        builder.Services.AddSingleton<ITokenService> (new TokenService());

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => options.TokenValidationParameters = ConfigureJwtBearer(builder));
        



        builder.Services.AddCors();
    }


    public static TokenValidationParameters ConfigureJwtBearer(WebApplicationBuilder builder)
    {
        return new() 
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new Exception("Отсутствует ключ!")))
        };
    }

    public static void Configure(WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        app.UseCors(builder =>
        {
            builder.WithOrigins("http://localhost:3000");
            builder.AllowCredentials();
            builder.AllowAnyHeader();
        });
    }

    public static void RegisterAPIs(WebApplication app)
    {
        new UserAPI().Register(app);
        new ProductAPI().Register(app);
        new AuthAPI().Register(app);
        new ProductPhotoAPI().Register(app);
        new CommentAPI().Register(app);
      /*  new ActivationAPI().Register(app);
        new CartAPI().Register(app);
        new CartProductAPI().Register(app);
        
        new FavouriteAPI().Register(app);
        new OrderAPI().Register(app);
        new OrderProductAPI().Register(app);
        
        new RateAPI().Register(app);*/
    }

}
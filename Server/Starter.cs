public static class Starter
{
    public static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
        });

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ITokenRepository, TokenRepository>();
        builder.Services.AddScoped<IRateRepository, RateRepository>();
        builder.Services.AddScoped<IProductPhotoRepository, ProductPhotoRepository>();
        builder.Services.AddScoped<IOrderProductRepository, OrderProductRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<ICartProductRepository, CartProductRepository>();
        builder.Services.AddScoped<ICartRepository, CartRepository>();
        builder.Services.AddScoped<IActivationRepository, ActivationRepository>();

        builder.Services.AddCors();
    }

    public static void Configure(WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();

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
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
        });
    }

    public static void RegisterAPIs(WebApplication app)
    {
        new UserAPI().Register(app);
        new ProductAPI().Register(app);
        new ActivationAPI().Register(app);
        new CartAPI().Register(app);
        new CartProductAPI().Register(app);
        new CommentAPI().Register(app);
        new FavouriteAPI().Register(app);
        new OrderAPI().Register(app);
        new OrderProductAPI().Register(app);
        new ProductPhotoAPI().Register(app);
        new RateAPI().Register(app);
        new TokenAPI().Register(app);
    }

}
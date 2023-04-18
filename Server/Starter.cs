public static class Starter
{
    public static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<UserDb>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
        });

        builder.Services.AddScoped<IUserRepository, UserRepository>();

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
            var db = scope.ServiceProvider.GetRequiredService<UserDb>();
            db.Database.EnsureCreated();
        }

        app.UseCors(builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
        });
    }

}
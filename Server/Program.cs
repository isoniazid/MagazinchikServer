
class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Starter.RegisterServices(builder);

        var app = builder.Build();

        Starter.Configure(app);

        new UserAPI().Register(app);

        app.Run();
    }
}
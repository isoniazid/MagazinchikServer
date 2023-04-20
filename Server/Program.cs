
class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Starter.RegisterServices(builder);

        var app = builder.Build();

        Starter.Configure(app);

        Starter.RegisterAPIs(app);

        app.Run();
    }
}
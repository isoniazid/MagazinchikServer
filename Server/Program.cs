
class Program
{
    public static void Main(string[] args)
    {
        Controller globalController = new Controller();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors();

        var app = builder.Build();

        app.UseCors(builder => {builder.AllowAnyOrigin();
         builder.AllowAnyHeader();});

        app.Run(globalController.APIHandler);
        

        app.Run();
    }
}
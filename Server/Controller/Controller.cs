

public class Controller
{
    public async Task SendUserInfo(HttpResponse response)
    {
        await response.WriteAsJsonAsync(new { id = 3, name = "Вова лох" });
    }

    public async Task SendTextMessage(HttpResponse response, string message)
    {
        await response.WriteAsync(message);
    }

    public async Task SendTextMessageJson(HttpResponse response, string message)
    {
        await response.WriteAsJsonAsync(new { message = message });
    }

    public async Task SendBackBody(HttpResponse response, HttpRequest request)
    {
        
        var bodyStr = "";

        // Allows using several time the stream in ASP.Net Core
        request.EnableBuffering(); 
        // Arguments: Stream, Encoding, detect encoding, buffer size 
        // AND, the most important: keep stream opened
        using (StreamReader reader 
                  = new StreamReader(request.Body, System.Text.Encoding.UTF8, true, 1024, true))
        {
            bodyStr = await reader.ReadToEndAsync();
        }

        // Rewind, so the core is not lost when it looks at the body for the request
        request.Body.Position = 0;

        // Do whatever works with bodyStr here

        await response.WriteAsync(bodyStr);
    }


    public async Task APIHandler(Microsoft.AspNetCore.Http.HttpContext context)
    {
        var response = context.Response;
        var request = context.Request;
        var path = request.Path;

        if (path == "/api/hello" && request.Method == "GET")
        {
            await SendTextMessageJson(response, "hello world");
        }

        if (path == "/api/vovchik" && request.Method == "GET")
        {
            await SendTextMessageJson(response, "BoB4uk /\\OX");
        }

        if (path == "/api/vasek" && request.Method == "GET")
        {
            await SendTextMessageJson(response, "Bacek Pecnekt");
        }

        if (path == "/api/user/1" && request.Method == "GET")
        {
            await SendUserInfo(response);
        }

        if (path == "/api/user/update" && request.Method == "POST")
        {
            await SendBackBody(response, request);
        }

        

    }

}
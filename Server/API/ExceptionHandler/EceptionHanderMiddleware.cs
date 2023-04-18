using System.Text.Json;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }

        catch (Exception ex)
        {
            await HandleMessageAsync(context, ex).ConfigureAwait(false);
        }
    }

    private static Task HandleMessageAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        int statusCode = (int)StatusCodes.Status404NotFound;

        var result = JsonSerializer.Serialize(new { StatucCode = statusCode, ErrorMessage = exception.Message });

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }

}
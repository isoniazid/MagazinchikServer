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

        catch (APIException ex)
        {
            await HandleAPIMessageAsync(context, ex).ConfigureAwait(false);
        }

        catch (Exception ex)
        {
            await HandleMessageAsync(context, ex).ConfigureAwait(false);
        }
    }

    private static Task HandleAPIMessageAsync(HttpContext context, APIException exception)
    // Ошибки сервера, связанные с API. Они вполне могут возникать
    {
        context.Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(new { StatusCode = exception.StatusCode, ErrorMessage = exception.Message });

        context.Response.StatusCode = exception.StatusCode;
        return context.Response.WriteAsync(result);
    }

    private static Task HandleMessageAsync(HttpContext context, Exception exception) 
    // Внутренние ошибки сервера. Их быть в норме не должно
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        return context.Response.WriteAsync(exception.ToString());
    }

}
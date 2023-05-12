public class ProductPhotoAPI
{
    public void Register(WebApplication app)
    {
        //Получить productPhoto по айди
        app.MapGet("/api/product_photo/{id}", (int id, HttpContext context) =>
        {
            var path = ($"{Directory.GetCurrentDirectory()}/img/{id}.jpg");
            try
            {
               return Results.File(path, contentType: "image/jpg");
            }

            catch (Exception ex)
            {
                throw ex;
            }
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetProductPhoto by id")
        .WithTags("productPhoto");

    }
}
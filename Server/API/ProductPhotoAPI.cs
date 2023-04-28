public class ProductPhotoAPI
{
    public void Register(WebApplication app)
    {
        //Получить всех productPhoto
        app.MapGet("/api/product_photo", async (IProductPhotoRepository repo) => Results.Ok(await repo.GetProductPhotosAsync()))
        .Produces<List<ProductPhoto>>(StatusCodes.Status200OK)
        .WithName("GetAllProductPhoto")
        .WithTags("productPhoto");

        //Получить productPhoto по айди
        app.MapGet("/api/product_photo/{id}", async (int id, IProductPhotoRepository repo) => await repo.GetProductPhotoAsync(id) is ProductPhoto productPhoto
        ? Results.Ok(productPhoto)
        : Results.NotFound())
        .Produces<ProductPhoto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetProductPhoto by id")
        .WithTags("productPhoto");

        //Добавить productPhoto с параметрами
        app.MapPost("/api/product_photo", async ([FromBody] ProductPhoto productPhoto, IProductPhotoRepository repo) =>
        {
            await repo.InsertProductPhotoAsync(productPhoto);
            await repo.SaveAsync();
            return Results.Created($"/api/{productPhoto.Id}", productPhoto);
        })
        .Accepts<ProductPhoto>("application/json")
        .Produces<ProductPhoto>(StatusCodes.Status201Created)
        .WithName("Adds productPhoto by params")
        .WithTags("productPhoto");

        //Изменить productPhoto
        app.MapPut("/api/product_photo", async ([FromBody] ProductPhoto productPhoto, IProductPhotoRepository repo) =>
        {
             
            await repo.UpdateProductPhotoAsync(productPhoto);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<ProductPhoto>("application/json")
        .WithName("Replaces productPhoto with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("productPhoto");

        //Удалить юзера
        app.MapDelete("/api/product_photo/{id}", async (int id, IProductPhotoRepository repo) =>
        {
            await repo.DeleteProductPhotoAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteProductPhoto")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("productPhoto");
    }
}
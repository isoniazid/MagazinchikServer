public class ProductAPI
{
    public void Register(WebApplication app)
    {
        //Получить всех product
        app.MapGet("/api/product", async (IProductRepository repo) => Results.Ok(await repo.GetProductsAsync()))
        .Produces<List<Product>>(StatusCodes.Status200OK)
        .WithName("GetAllProduct")
        .WithTags("product");

        //Получить product по айди
        app.MapGet("/api/product/{id}", async (int id, IProductRepository repo) => await repo.GetProductAsync(id) is Product product
        ? Results.Ok(product)
        : Results.NotFound())
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetProduct by id")
        .WithTags("product");

        //Добавить product с параметрами
        app.MapPost("/api/product", async ([FromBody] Product product, IProductRepository repo) =>
        {
            await repo.InsertProductAsync(product);
            await repo.SaveAsync();
            return Results.Created($"/api/{product.Id}", product);
        })
        .Accepts<Product>("application/json")
        .Produces<Product>(StatusCodes.Status201Created)
        .WithName("Adds product by params")
        .WithTags("product");

        //Изменить product
        app.MapPut("/api/product", async ([FromBody] Product product, IProductRepository repo) =>
        {
            await repo.UpdateProductAsync(product);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<Product>("application/json")
        .WithName("Replaces product with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("product");

        //Удалить юзера
        app.MapDelete("/api/product/{id}", async (int id, IProductRepository repo) =>
        {
            await repo.DeleteProductAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteProduct")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("product");
    }
}
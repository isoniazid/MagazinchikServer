public class CartProductAPI
{
    public void Register(WebApplication app)
    {

        //Получить всех cartProduct
        app.MapGet("/api/cart_product", async (ICartProductRepository repo) => Results.Ok(await repo.GetCartProductsAsync()))
        .Produces<List<CartProduct>>(StatusCodes.Status200OK)
        .WithName("GetAllCartProduct")
        .WithTags("cartProduct");

        //Получить cartProduct по айди
        app.MapGet("/api/cart_product/{id}", async (int id, ICartProductRepository repo) => await repo.GetCartProductAsync(id) is CartProduct cartProduct
        ? Results.Ok(cartProduct)
        : Results.NotFound())
        .Produces<CartProduct>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetCartProduct by id")
        .WithTags("cartProduct");

        //Добавить cartProduct с параметрами
        app.MapPost("/api/cart_product", async ([FromBody] CartProduct cartProduct, ICartProductRepository repo) =>
        {
            await repo.InsertCartProductAsync(cartProduct);
            await repo.SaveAsync();
            return Results.Created($"/api/{cartProduct.Id}", cartProduct);
        })
        .Accepts<CartProduct>("application/json")
        .Produces<CartProduct>(StatusCodes.Status201Created)
        .WithName("Adds cartProduct by params")
        .WithTags("cartProduct");

        //Изменить cartProduct
        app.MapPut("/api/cart_product", async ([FromBody] CartProduct cartProduct, ICartProductRepository repo) =>
        {
             
            await repo.UpdateCartProductAsync(cartProduct);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<CartProduct>("application/json")
        .WithName("Replaces cartProduct with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("cartProduct");

        //Удалить юзера
        app.MapDelete("/api/cart_product/{id}", async (int id, ICartProductRepository repo) =>
        {
            await repo.DeleteCartProductAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteCartProduct")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("cartProduct");
    }
}
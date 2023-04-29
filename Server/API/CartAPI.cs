public class CartAPI
{
    public void Register(WebApplication app)
    {
        //Получить всех cart
        app.MapGet("/api/cart", async (ICartRepository repo) => Results.Ok(await repo.GetCartsAsync()))
        .Produces<List<Cart>>(StatusCodes.Status200OK)
        .WithName("GetAllCart")
        .WithTags("cart");

        //Получить cart по айди
        app.MapGet("/api/cart/{id}", async (int id, ICartRepository repo) => await repo.GetCartAsync(id) is Cart cart
        ? Results.Ok(cart)
        : Results.NotFound())
        .Produces<Cart>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetCart by id")
        .WithTags("cart");

        //Добавить cart с параметрами
        app.MapPost("/api/cart", async ([FromBody] Cart cart, ICartRepository repo) =>
        {
            await repo.InsertCartAsync(cart);
            await repo.SaveAsync();
            return Results.Created($"/api/{cart.Id}", cart);
        })
        .Accepts<Cart>("application/json")
        .Produces<Cart>(StatusCodes.Status201Created)
        .WithName("Adds cart by params")
        .WithTags("cart");

        //Изменить cart
        app.MapPut("/api/cart", async ([FromBody] Cart cart, ICartRepository repo) =>
        {
            await repo.UpdateCartAsync(cart);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<Cart>("application/json")
        .WithName("Replaces cart with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("cart");

        //Удалить юзера
        app.MapDelete("/api/cart/{id}", async (int id, ICartRepository repo) =>
        {
            await repo.DeleteCartAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteCart")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("cart");
    }
}
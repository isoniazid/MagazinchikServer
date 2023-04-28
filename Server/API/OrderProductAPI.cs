public class OrderProductAPI
{
    public void Register(WebApplication app)
    {

        return;

        //Получить всех orderProduct
        app.MapGet("/api/order_product", async (IOrderProductRepository repo) => Results.Ok(await repo.GetOrderProductsAsync()))
        .Produces<List<OrderProduct>>(StatusCodes.Status200OK)
        .WithName("GetAllOrderProduct")
        .WithTags("orderProduct");

        //Получить orderProduct по айди
        app.MapGet("/api/order_product/{id}", async (int id, IOrderProductRepository repo) => await repo.GetOrderProductAsync(id) is OrderProduct orderProduct
        ? Results.Ok(orderProduct)
        : Results.NotFound())
        .Produces<OrderProduct>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetOrderProduct by id")
        .WithTags("orderProduct");

        //Добавить orderProduct с параметрами
        app.MapPost("/api/order_product", async ([FromBody] OrderProduct orderProduct, IOrderProductRepository repo) =>
        {
            await repo.InsertOrderProductAsync(orderProduct);
            await repo.SaveAsync();
            return Results.Created($"/api/{orderProduct.Id}", orderProduct);
        })
        .Accepts<OrderProduct>("application/json")
        .Produces<OrderProduct>(StatusCodes.Status201Created)
        .WithName("Adds orderProduct by params")
        .WithTags("orderProduct");

        //Изменить orderProduct
        app.MapPut("/api/order_product", async ([FromBody] OrderProduct orderProduct, IOrderProductRepository repo) =>
        {
             
            await repo.UpdateOrderProductAsync(orderProduct);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<OrderProduct>("application/json")
        .WithName("Replaces orderProduct with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("orderProduct");

        //Удалить юзера
        app.MapDelete("/api/order_product/{id}", async (int id, IOrderProductRepository repo) =>
        {
            await repo.DeleteOrderProductAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteOrderProduct")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("orderProduct");
    }
}
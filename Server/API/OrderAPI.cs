public class OrderAPI
{
    public void Register(WebApplication app)
    {

        return;

        //Получить всех order
        app.MapGet("/api/order", async (IOrderRepository repo) => Results.Ok(await repo.GetOrdersAsync()))
        .Produces<List<Order>>(StatusCodes.Status200OK)
        .WithName("GetAllOrder")
        .WithTags("order");

        //Получить order по айди
        app.MapGet("/api/order/{id}", async (int id, IOrderRepository repo) => await repo.GetOrderAsync(id) is Order order
        ? Results.Ok(order)
        : Results.NotFound())
        .Produces<Order>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetOrder by id")
        .WithTags("order");

        //Добавить order с параметрами
        app.MapPost("/api/order", async ([FromBody] Order order, IOrderRepository repo) =>
        {
            await repo.InsertOrderAsync(order);
            await repo.SaveAsync();
            return Results.Created($"/api/{order.Id}", order);
        })
        .Accepts<Order>("application/json")
        .Produces<Order>(StatusCodes.Status201Created)
        .WithName("Adds order by params")
        .WithTags("order");

        //Изменить order
        app.MapPut("/api/order", async ([FromBody] Order order, IOrderRepository repo) =>
        {
             
            await repo.UpdateOrderAsync(order);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<Order>("application/json")
        .WithName("Replaces order with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("order");

        //Удалить юзера
        app.MapDelete("/api/order/{id}", async (int id, IOrderRepository repo) =>
        {
            await repo.DeleteOrderAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteOrder")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("order");
    }
}
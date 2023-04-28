public class ActivationAPI
{
    public void Register(WebApplication app)
    {
        //Получить всех activation
        app.MapGet("/api/activation", async (IActivationRepository repo) => Results.Ok(await repo.GetActivationsAsync()))
        .Produces<List<Activation>>(StatusCodes.Status200OK)
        .WithName("GetAllActivation")
        .WithTags("activation");

        //Получить activation по айди
        app.MapGet("/api/activation/{id}", async (int id, IActivationRepository repo) => await repo.GetActivationAsync(id) is Activation activation
        ? Results.Ok(activation)
        : Results.NotFound())
        .Produces<Activation>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetActivation by id")
        .WithTags("activation");

        //Добавить activation с параметрами
        app.MapPost("/api/activation", async ([FromBody] Activation activation, IActivationRepository repo) =>
        {
            await repo.InsertActivationAsync(activation);
            await repo.SaveAsync();
            return Results.Created($"/api/{activation.Id}", activation);
        })
        .Accepts<Activation>("application/json")
        .Produces<Activation>(StatusCodes.Status201Created)
        .WithName("Adds activation by params")
        .WithTags("activation");

        //Изменить activation
        app.MapPut("/api/activation", async ([FromBody] Activation activation, IActivationRepository repo) =>
        {
            
            await repo.UpdateActivationAsync(activation);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<Activation>("application/json")
        .WithName("Replaces activation with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("activation");

        //Удалить юзера
        app.MapDelete("/api/activation/{id}", async (int id, IActivationRepository repo) =>
        {
            await repo.DeleteActivationAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteActivation")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("activation");
    }
}
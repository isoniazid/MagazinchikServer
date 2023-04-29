public class RateAPI
{
    public void Register(WebApplication app)
    {



        //Получить всех rate
        app.MapGet("/api/rate", async (IRateRepository repo) => Results.Ok(await repo.GetRatesAsync()))
        .Produces<List<Rate>>(StatusCodes.Status200OK)
        .WithName("GetAllRate")
        .WithTags("rate");

        //Получить rate по айди
        app.MapGet("/api/rate/{id}", async (int id, IRateRepository repo) => await repo.GetRateAsync(id) is Rate rate
        ? Results.Ok(rate)
        : Results.NotFound())
        .Produces<Rate>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetRate by id")
        .WithTags("rate");

        //Добавить rate с параметрами
        app.MapPost("/api/rate", async ([FromBody] Rate rate, IRateRepository repo) =>
        {
            await repo.InsertRateAsync(rate);
            await repo.SaveAsync();
            return Results.Created($"/api/{rate.Id}", rate);
        })
        .Accepts<Rate>("application/json")
        .Produces<Rate>(StatusCodes.Status201Created)
        .WithName("Adds rate by params")
        .WithTags("rate");

        //Изменить rate
        app.MapPut("/api/rate", async ([FromBody] Rate rate, IRateRepository repo) =>
        {

            await repo.UpdateRateAsync(rate);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<Rate>("application/json")
        .WithName("Replaces rate with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("rate");

        //Удалить юзера
        app.MapDelete("/api/rate/{id}", async (int id, IRateRepository repo) =>
        {
            await repo.DeleteRateAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteRate")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("rate");
    }
}
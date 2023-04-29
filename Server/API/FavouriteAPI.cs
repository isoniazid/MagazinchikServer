public class FavouriteAPI
{
    public void Register(WebApplication app)
    {

  

        //Получить всех favourite
        app.MapGet("/api/favourite", async (IFavouriteRepository repo) => Results.Ok(await repo.GetFavouritesAsync()))
        .Produces<List<Favourite>>(StatusCodes.Status200OK)
        .WithName("GetAllFavourite")
        .WithTags("favourite");

        //Получить favourite по айди
        app.MapGet("/api/favourite/{id}", async (int id, IFavouriteRepository repo) => await repo.GetFavouriteAsync(id) is Favourite favourite
        ? Results.Ok(favourite)
        : Results.NotFound())
        .Produces<Favourite>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetFavourite by id")
        .WithTags("favourite");

        //Добавить favourite с параметрами
        app.MapPost("/api/favourite", async ([FromBody] Favourite favourite, IFavouriteRepository repo) =>
        {
            await repo.InsertFavouriteAsync(favourite);
            await repo.SaveAsync();
            return Results.Created($"/api/{favourite.Id}", favourite);
        })
        .Accepts<Favourite>("application/json")
        .Produces<Favourite>(StatusCodes.Status201Created)
        .WithName("Adds favourite by params")
        .WithTags("favourite");

        //Изменить favourite
        app.MapPut("/api/favourite", async ([FromBody] Favourite favourite, IFavouriteRepository repo) =>
        {
             
            await repo.UpdateFavouriteAsync(favourite);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .Accepts<Favourite>("application/json")
        .WithName("Replaces favourite with params")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("favourite");

        //Удалить юзера
        app.MapDelete("/api/favourite/{id}", async (int id, IFavouriteRepository repo) =>
        {
            await repo.DeleteFavouriteAsync(id);
            await repo.SaveAsync();
            return Results.Ok();
        })
        .WithName("DeleteFavourite")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("favourite");
    }
}
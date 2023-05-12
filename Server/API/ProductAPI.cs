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
        app.MapGet("/api/product/{id}", async (int id, IProductRepository repo) => await repo.GetProductDtoAsync(id) is ProductDto product
        ? Results.Ok(product)
        : Results.NotFound())
        .Produces<ProductDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetProduct by id")
        .WithTags("product");

        //получить product по слагу
        app.MapGet("/api/product/slug/{slug}", async (string slug, IProductRepository repo) => await repo.GetProductDtoAsync(slug) is ProductDto product
        ? Results.Ok(product)
        : Results.NotFound())
        .Produces<ProductDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetProduct by slug")
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


        //Добавить productPhoto с параметрами
        app.MapPost("/api/product/photo/{id}", async (int id, IFormFile fileToUpload, IProductPhotoRepository repo, IProductRepository productRepo) =>
        {
            var currentProduct = await productRepo.GetProductAsync(id) ?? throw new APIException("product not found", 400);
            var photo = new ProductPhoto();


            photo.Id = await repo.GetLastId() + 1;
            currentProduct.Photos.Add(photo);
            photo.Product = currentProduct;

            string uploadPath = $"{Directory.GetCurrentDirectory()}/img/{photo.Id}.jpg";


            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                await fileToUpload.CopyToAsync(fileStream);
            }


            await repo.InsertProductPhotoAsync(photo);
            await repo.SaveAsync();
            await productRepo.UpdateProductAsync(currentProduct);
            await productRepo.SaveAsync();



            return Results.Created($"/api/{photo.Id}", photo);
        })
        .Produces<ProductPhoto>(StatusCodes.Status201Created)
        .WithName("Adds productPhoto by params")
        .WithTags("product");
    }
}
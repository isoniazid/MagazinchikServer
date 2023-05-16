public class CartProductAPI
{
    public void Register(WebApplication app)
    {

        app.MapGet("api/cart_product/cart/{cart_id}", (int cart_id, IProductRepository repo) => 
        {
            return repo.GetAllProductsByCartId(cart_id);
        })
        .Produces<List<Product>>(StatusCodes.Status200OK)
        .WithName("get all products in cart")
        .WithTags("cartProduct");

        app.MapPost("api/cart_product", async ([FromBody] CartProductDto dto, ICartProductRepository repo) => 
        {
            var cartProductToLoad = new CartProduct {CartId = dto.cartId, ProductCount = dto.productCount, ProductId = dto.productId};
            await repo.InsertCartProductAsync(cartProductToLoad);
            await repo.SaveAsync();
            return Results.Created($"/api/{cartProductToLoad.Id}", cartProductToLoad);
        })
        .Produces<CartProduct>(StatusCodes.Status200OK)
        .WithName("Post CartProduct")
        .WithTags("cartProduct");

        //Удалить товар из корзины
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

        //удалить все товары из корзины по Карт айди
        app.MapDelete("/api/cart_product/cart/{cart_id}", async (int cart_id, ICartProductRepository repo) => 
        {
            var result = repo.DeleteAllCartProductAsync(cart_id);
            await repo.SaveAsync();
            return Results.Ok(result);
        })
        .WithName("DeleteAllProductsByCartId")
        .Produces<List<ProductDto>>(StatusCodes.Status200OK)
        .WithTags("cartProduct");

        app.MapPatch("/api/cart_product", async ([FromBody] CartProductDto dto, ICartProductRepository repo) =>
        {
            var result = await repo.UpdateCartProductAsync(dto);
            await repo.SaveAsync();
            return result;
        })
        .WithName("Patchproduct")
        .Produces<CartProduct>(StatusCodes.Status200OK)
        .WithTags("cartProduct");

    }
}
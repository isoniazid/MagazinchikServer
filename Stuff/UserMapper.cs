/* public class UserMapper : IModelDTO<User, UserDTO>
{
    public UserDTO ToDTO(User source)
    {
        UserDTO result = new UserDTO();

        result.Id = source.Id;
        result.Email = source.Email;
        result.Password = source.Password;
        result.Name = source.Name;
        result.Role = source.Role;

        if (source.Orders != null) foreach (var element in source.Orders) result?.OrdersId?.Add(element.Id);

        result.CartId = source?.Cart?.Id;

        if (source?.Favourites != null) foreach (var element in source.Favourites) result?.FavouritesId?.Add(element.Id);

        if (source?.Comments != null) foreach (var element in source.Comments) result?.CommentsId?.Add(element.Id);

        if (source?.Rates != null) foreach (var element in source.Rates) result?.RatesId?.Add(element.Id);

        result.ActivationId = source?.Activation?.Id;

        result.TokenId = source?.Token?.Id;


        return result;
    }

    public User ToModel(UserDTO source)
    {
        User result = new User();

        result.Id = source.Id;
        result.Email = source.Email;
        result.Password = source.Password;
        result.Name = source.Name;
        result.Role = source.Role;

        if (source.OrdersId != null) foreach (var element in source.OrdersId) result?.Orders?.Id = element;

        if (source.CartId != null)
        {
            result.Cart = new Cart();
            result.Cart.Id = (int)source.CartId;
        }

        if (source?.Favourites != null) foreach (var element in source.Favourites) result?.FavouritesId?.Add(element.Id);

        if (source?.Comments != null) foreach (var element in source.Comments) result?.CommentsId?.Add(element.Id);

        if (source?.Rates != null) foreach (var element in source.Rates) result?.RatesId?.Add(element.Id);

        result.ActivationId = source?.Activation?.Id;

        result.TokenId = source?.Token?.Id;

        return result;
    }
} */
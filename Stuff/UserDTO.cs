public class UserDTO
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty; //NB возможно тут нужен енум

    public List<int>? OrdersId { get; set; } = new();

    public int? CartId { get; set; } = new();

    public List<int>? FavouritesId { get; set; } = new();

    public List<int>? CommentsId { get; set; } = new();

    public List<int>? RatesId { get; set; } = new();

    public int? ActivationId { get; set; } = new();
    public int? TokenId { get; set; } = new();
}


public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty; //NB возможно тут нужен енум

/*     public List<Order>? Orders { get; set; } = new();

    public Cart? Cart { get; set; } = new();

    public List<Favourite>? Favourites { get; set; } = new();

    public List<Comment>? Comments { get; set; } = new();

    public List<Rate>? Rates { get; set; } = new();

    public Activation? Activation { get; set; } = new();

    public Token? Token { get; set; } = new(); */


}
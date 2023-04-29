public class Comment : Traceable
{
    public int Id { get; private set; }
    public string Text { get; set; } = String.Empty;
    /* public int UserId { get; set; } */
    public User? User { get; set; }
    /* public int ProductId { get; set; } */

    public int ProductId {get; set;}

    [JsonIgnore]
    public Product? Product;
}
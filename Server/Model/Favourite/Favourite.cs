public class Favourite : Traceable
{
    public int Id { get; private set; }
    /* public int UserId { get; set; } */
    public User? User { get; set; }
    /* public int ProductId { get; set; } */
    public Product? Product { get; set; }
}
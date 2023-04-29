public class CartProduct : Traceable
{
    public int Id { get; private set; }
    public int ProductCount { get; set; }
    /* public int CartId { get; set; } */
    public Cart? Cart { get; set; }
    /* public int ProductId {get; set; } */
    public Product? Product {get; set;}
}
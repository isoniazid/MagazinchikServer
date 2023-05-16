public class OrderProduct : Traceable
{
    public int Id { get; private set; }
    public int ProductCount { get; set; }
    /* public int OrderId { get; set; } */
    public Order? Order { get; set; }
    /* public int ProductId { get; set; } */
    public Product? Product { get; set; }
}
public class ProductPhoto : Traceable
{
    public int Id { get; private set; }
    //public int ProductId { get; set; }
    public Product? Product { get; set; }
}
public class ProductPhoto : Traceable
{
    public int Id { get; set; }
    //public int ProductId { get; set; }
    public Product? Product { get; set; }
}
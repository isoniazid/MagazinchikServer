public class ProductPhoto : Traceable
{
    public int Id { get; set; }
    
    
    public int ProductId { get; set; }

    [JsonIgnore]
    public Product? Product { get; set; }
}
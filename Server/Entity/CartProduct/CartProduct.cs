public class CartProduct : Traceable
{
    public int Id { get; private set; }
    public int ProductCount { get; set; }
    public int CartId { get; set; }

    [JsonIgnore]
    public int ProductId { get; set; }

    [JsonIgnore]
    public Product? Product { get; set; }
}
public class Cart : Traceable
{
    public int Id { get; private set; }

    [JsonIgnore]
    public User? User { get; set; }
    
    [JsonIgnore]
    public List<CartProduct> CartProducts = new();

}
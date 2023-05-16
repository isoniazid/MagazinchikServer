public class Rate : Traceable
{
    public int Id { get; private set; }
    public int Value { get; set; } //NB ENUM?
    //public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
    //public int ProductId { get; set; }

    public int ProductId {get; set;}

    [JsonIgnore]
    public Product? Product { get; set; }
}
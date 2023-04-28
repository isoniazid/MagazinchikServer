

public class Order : Traceable
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = String.Empty; //NB Enum?
   /*  public int UserId { get; set; } */
   [JsonIgnore]
    public User? User { get; set; }
   /*  public List<OrderProduct> OrderProducts { get; set; } = new(); */
}
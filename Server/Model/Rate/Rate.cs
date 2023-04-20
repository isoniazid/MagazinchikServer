public class Rate
{
    public int Id { get; set; }
    public int Value { get; set; } //NB ENUM?
    //public int UserId { get; set; }
    public User? User { get; set; }
    //public int ProductId { get; set; }
    public Product? Product { get; set; }
}
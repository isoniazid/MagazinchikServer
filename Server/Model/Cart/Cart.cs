public class Cart : Traceable
{
    public int Id { get; private set; }
    /* public int UserId { get; set; } */
    public User User { get; set; } = new();
    /* public List<CartProduct> CartProducts = new(); */

}
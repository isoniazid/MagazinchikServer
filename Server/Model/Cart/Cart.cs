public class Cart
{
    public int Id { get; set; }
    /* public int UserId { get; set; } */
    public User User { get; set; } = new();
    /* public List<CartProduct> CartProducts = new(); */

}
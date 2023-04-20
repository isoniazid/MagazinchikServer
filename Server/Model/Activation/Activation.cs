public class Activation
{
    public int Id { get; set; }
    public bool IsActivated { get; set; }
    public string Link { get; set; } = string.Empty;
    /* public int UserId { get; set; } */
    public User? User { get; set; }
}
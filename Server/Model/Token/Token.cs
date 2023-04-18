public class Token
{
    public int Id { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User? User { get; set; }
}
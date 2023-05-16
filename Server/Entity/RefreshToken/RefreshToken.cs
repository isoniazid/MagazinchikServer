public class RefreshToken
{
    
    public int Id { get; private set; }
    public string Value { get; set; } = string.Empty;
    //public int UserId { get; set; }
    [Required]
    public DateTime Expires { get; set; }

    [Required] [JsonIgnore]
    public User User { get; set; } = new();
}
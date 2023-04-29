public class User : Traceable
{
    public int Id { get; set; }

    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public string Role { get; set; } = string.Empty; 
}
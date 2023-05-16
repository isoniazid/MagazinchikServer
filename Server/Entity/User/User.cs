public class User : Traceable
{
    public int Id { get; private set; }

    [Required]
    public string Email { get; set; } = string.Empty;
    [Required] [JsonIgnore]
    public string Password { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public string Role { get; set; } = string.Empty; 

    public int CartId {get; set;}

    [JsonIgnore]
    public Cart Cart {get; set;} = new();
}
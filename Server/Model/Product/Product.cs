public class Product
{

    public int Id { get; set; }

    private string _name = string.Empty;
    public string Name
    {
        get {return _name;}
        set 
        {
            _name = value;
            Slug = value;
        }
    }

    private string _slug = string.Empty;

    public string Slug 
    {
        get { return _slug; }
        private set
        {
            _slug = value.GenerateSlug("_");
        }
    } 
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;

    public int CommentsCount { get; set; }

    public int AverageRating { get; set; }

}
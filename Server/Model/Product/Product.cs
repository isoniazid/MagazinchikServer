public class Product : Traceable
{

    public int Id { get; private set; }

    private string _name = string.Empty;
    public string Name
    {
        get { return _name; }
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

    public int CommentsCount
    {
        get { return Comments.Count(); }
        private set { }
    }


    [JsonIgnore]
    public List<Comment> Comments { get; set; } = new();
    public int AverageRating
    {
        get
        {
            if (Rates.Count == 0) return 0;
            return (Rates.Sum(x => x.Value) / Rates.Count());
        }
        private set { }
    }

    [JsonIgnore]
    public List<Rate> Rates { get; set; } = new();

}


public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;

    public int CommentsCount {get; set;}

    public int AverageRating {get; set;}
    /* public int CommentsCount
    {
        get
        {
            return Comments.Count();
        }
        set { CommentsCount = value; }
    }
    public int AverageRating
    {
        get
        {
            AverageRating = 0;
            foreach (var rate in Rates) AverageRating += rate.Value;
            AverageRating /= Rates.Count;
            return AverageRating;
        }
        set { AverageRating = value; } //NB возможно сделал какую-то фигню
    } */
    /* public List<OrderProduct> Orders { get; set; } = new();
    public List<CartProduct> Carts { get; set; } = new();
    public List<Favourite> Favourites { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<Rate> Rates { get; set; } = new();
    public List<ProductPhoto> Photos { get; set; } = new();
 */
}
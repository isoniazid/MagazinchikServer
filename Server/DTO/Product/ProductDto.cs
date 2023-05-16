public record ProductDto (
int id,
string name,
string slug,
decimal price,
string description,
int CommentsCount,
int averageRating,
int [] photos
);
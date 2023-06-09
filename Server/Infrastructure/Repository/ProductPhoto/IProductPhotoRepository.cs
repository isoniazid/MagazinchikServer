public interface IProductPhotoRepository : IDisposable
{
/*     Task <List<ProductPhoto>> GetProductPhotosAsync();
 */
    Task<ProductPhoto> GetProductPhotoAsync(int productPhotoId);

    Task InsertProductPhotoAsync(ProductPhoto productPhoto);

    Task<int> GetLastId();

/*     Task UpdateProductPhotoAsync(ProductPhoto productPhoto);

    Task DeleteProductPhotoAsync(int productPhotoId); */

    Task SaveAsync();
}
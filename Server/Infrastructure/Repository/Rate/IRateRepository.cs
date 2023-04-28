public interface IRateRepository : IDisposable
{
    Task <List<Rate>> GetRatesAsync();

    Task<Rate> GetRateAsync(int rateId);

    Task InsertRateAsync(Rate rate);

    Task UpdateRateAsync(Rate rate);

    Task DeleteRateAsync(int rateId);

    Task SaveAsync();
}
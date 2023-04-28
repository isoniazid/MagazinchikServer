public class RateRepository : IRateRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public RateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteRateAsync(int rateId)
    {
        var rateFromDb = await _context.Rates.FindAsync(new object[] { rateId });
        if (rateFromDb == null) throw new APIException($"No such rate with id {rateId}", StatusCodes.Status404NotFound);
        _context.Rates.Remove(rateFromDb);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<Rate> GetRateAsync(int rateId)
    {
        var result = await _context.Rates.FindAsync(new object[] { rateId });
        if (result == null) throw new APIException("No such rate", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<Rate>> GetRatesAsync()
    {
        return await _context.Rates.ToListAsync();
    }

    public async Task InsertRateAsync(Rate rate)
    {
        await _context.Rates.AddAsync(rate);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRateAsync(Rate rate)
    {
        var rateFromDb = await _context.Rates.FindAsync(new object[] { rate.Id });
        if (rateFromDb == null) throw new APIException("No such rate", StatusCodes.Status404NotFound);
        
        foreach(var prop in typeof(Rate).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(rateFromDb, prop.GetValue(rate)); //NB обобщил
        }
    }
}
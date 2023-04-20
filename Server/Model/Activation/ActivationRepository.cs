public class ActivationRepository : IActivationRepository
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;
    public ActivationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteActivationAsync(int activationId)
    {
        var activationFromDb = await _context.Activations.FindAsync(new object[] { activationId });
        if (activationFromDb == null) throw new APIException($"No such activation with id {activationId}", StatusCodes.Status404NotFound);
        _context.Activations.Remove(activationFromDb);
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

    public async Task<Activation> GetActivationAsync(int activationId)
    {
        var result = await _context.Activations.FindAsync(new object[] { activationId });
        if (result == null) throw new APIException("No such activation", StatusCodes.Status404NotFound);
        else return result;
    }
    public async Task<List<Activation>> GetActivationsAsync()
    {
        return await _context.Activations.ToListAsync();
    }

    public async Task InsertActivationAsync(Activation activation)
    {
        await _context.Activations.AddAsync(activation);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateActivationAsync(Activation activation)
    {
        var activationFromDb = await _context.Activations.FindAsync(new object[] { activation.Id });
        if (activationFromDb == null) throw new APIException("No such activation", StatusCodes.Status404NotFound);
        
        foreach(var prop in typeof(Activation).GetProperties(BindingFlags.Public))
        {
            prop.SetValue(activationFromDb, prop.GetValue(activation)); //NB обобщил
        }
    }
}
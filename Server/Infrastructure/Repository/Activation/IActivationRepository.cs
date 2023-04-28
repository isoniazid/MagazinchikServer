public interface IActivationRepository : IDisposable
{
    Task <List<Activation>> GetActivationsAsync();

    Task<Activation> GetActivationAsync(int activationId);

    Task InsertActivationAsync(Activation activation);

    Task UpdateActivationAsync(Activation activation);

    Task DeleteActivationAsync(int activationId);

    Task SaveAsync();
}
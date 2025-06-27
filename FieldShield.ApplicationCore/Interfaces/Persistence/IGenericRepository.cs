
namespace FieldShield.ApplicationCore.Interfaces.Persistence;
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken token = default);
    Task<T> AddAsync(T entity, CancellationToken token = default);
    Task UpdateAsync(T entity, CancellationToken token = default);
    Task DeleteAsync(Guid id, CancellationToken token = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken token = default);
}

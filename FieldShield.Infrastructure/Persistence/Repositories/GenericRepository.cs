
using FieldShield.ApplicationCore.Entities;
using FieldShield.ApplicationCore.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FieldShield.Infrastructure.Persistence.Repositories;
internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly FileShieldDbContext _context;

    public GenericRepository(FileShieldDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken token = default)
    {
        await _context.Set<TEntity>().AddAsync(entity, token);
        await _context.SaveChangesAsync(token);

        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var item = _context.Set<TEntity>().Find(id);

        if(item is not null)
        {
            _context.Remove(item);
            await _context.SaveChangesAsync(token);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Set<TEntity>().AnyAsync(e => e.Id == id, token);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken token = default)
    {
        return await _context.Set<TEntity>().AsNoTracking().ToListAsync(token);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, token);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken token = default)
    {
        if(await _context.Set<TEntity>().AnyAsync(e => e.Id == entity.Id, token))
        {
            _context.Update(entity);
            await _context.SaveChangesAsync(token);
        }

    }
}

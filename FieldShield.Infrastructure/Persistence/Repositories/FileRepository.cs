
using FieldShield.ApplicationCore.Interfaces.Persistence;

namespace FieldShield.Infrastructure.Persistence.Repositories;
internal class FileRepository : GenericRepository<ApplicationCore.Entities.File>, IFileRepository
{
    public FileRepository(FileShieldDbContext context) : base(context)
    {
    }
}

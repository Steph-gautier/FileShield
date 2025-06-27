

using FieldShield.ApplicationCore.Entities;
using FieldShield.ApplicationCore.Interfaces.Persistence;

namespace FieldShield.Infrastructure.Persistence.Repositories;
internal class FolderRepository : GenericRepository<Folder>, IFolderRepository
{
    public FolderRepository(FileShieldDbContext context) : base(context)
    {
    }
}

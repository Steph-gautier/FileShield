using FieldShield.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace FieldShield.Infrastructure.Persistence
{
    public class FileShieldDbContext : DbContext
    {
        public FileShieldDbContext(DbContextOptions<FileShieldDbContext> options)
            : base(options)
        {
        }

        public DbSet<Folder> Folders { get; set; }
        public DbSet<ApplicationCore.Entities.File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseEntity>()
                .UseTpcMappingStrategy()
                .HasQueryFilter(entity => !entity.IsDeleted && !entity.DeletedAt.HasValue);

            modelBuilder.Entity<Folder>()
                .HasOne(f => f.ParentFolder)
                .WithMany(f => f.SubFolders)
                .HasForeignKey(f => f.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationCore.Entities.File>()
                .HasOne(f => f.ParentFolder)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.FolderId);

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileShieldDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = base.ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added 
                || e.State == EntityState.Modified
                || e.State == EntityState.Deleted
                );

            foreach (var entry in entries)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;

                if(entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.IsDeleted = true;
                    entry.State = EntityState.Modified; // Change state to Modified to avoid actual deletion
                }

                if(entry.State == EntityState.Modified)
                {
                   entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

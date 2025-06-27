

namespace FieldShield.ApplicationCore.Entities;
public class Folder : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public int Order { get; set; }
    public Guid? ParentId { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime LastDateOpened { get; set; }

    public Folder? ParentFolder { get; set; }
    public ICollection<Folder> SubFolders { get; set; } = new List<Folder>();
    public ICollection<File> Files { get; set; } = new List<File>();
}

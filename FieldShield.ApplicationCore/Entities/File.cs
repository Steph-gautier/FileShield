

namespace FieldShield.ApplicationCore.Entities;
public class File : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public long Size { get; set; }
    public string? Description { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public Guid FolderId { get; set; }
    public Guid OwnerId { get; set; }

    public Folder ParentFolder { get; set; } = null!;
}

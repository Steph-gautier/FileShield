using System.Text.Json.Nodes;

namespace FieldShield.SeawedFileAPI.Models
{
    public class FileMetadata
    {
        public string Path { get; set; }
        public IEnumerable<Entries> Entries { get; set; }
        public long Limit { get; set; }
        public string LastFileName { get; set; }
        public bool ShouldDisplayLoadMore { get; set; }
    }
    public class Entries
    {
        public string FullPath { get; set; }
        public string Mtime { get; set; }
        public string Crtime { get; set; }
        public long Mode { get; set; }
        public long Uid { get; set; }
        public long Gid { get; set; }
        public string Mime { get; set; }
        public string Replication { get; set; }
        public string Collection { get; set; } = string.Empty;
        public long TtlSec { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string GroupNames { get; set; }
        public string SymlinkTarget { get; set; }
        public string Md5 { get; set; }
        public string Extended { get; set; }
        public IEnumerable<JsonObject> chuncks { get; set; }
    }
}

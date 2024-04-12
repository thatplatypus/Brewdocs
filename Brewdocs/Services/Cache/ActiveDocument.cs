using System;
namespace Brewdocs.Services.Cache
{
    public class ActiveDocument
    {
        public string Name { get; set; } = "Untitled";

        public string DocumentType { get; set; }

        public byte[] Data { get; set; }

        public DateTime LastAccessed { get; set; } = DateTime.Now;

        public string ShortName => Name.IndexOf('.') > 0 ? Name[..Name.IndexOf('.')] : Name;

        public string FileExtension => Name.Contains('.') ? Name[(Name.LastIndexOf('.') + 1)..] : string.Empty;
    }
}


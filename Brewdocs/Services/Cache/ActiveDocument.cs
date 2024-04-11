using System;
namespace Brewdocs.Services.Cache
{
    public class ActiveDocument
    {
        public string Name { get; set; } = "Untitled";

        public string DocumentType { get; set; }

        public byte[] Data { get; set; }

        public string ShortName => Name.IndexOf('.') > 0 ? Name[..Name.IndexOf('.')] : Name;
    }
}


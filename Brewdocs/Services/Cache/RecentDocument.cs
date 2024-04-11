using System;
namespace Brewdocs.Services.Cache
{
    public class RecentDocument
    {
        public string Path { get; set; }

        public ActiveDocument Document { get; set; }
    }
}


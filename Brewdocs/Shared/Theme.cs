using System;
namespace Brewdocs.Shared
{
    public class Theme
    {
        public string Color { get; set; }

        public string ColorCssStyle => $"background-color:{Color}";
    }
}


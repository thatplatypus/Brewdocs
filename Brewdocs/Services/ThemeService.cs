using System;
namespace Brewdocs.Services
{
    public class ThemeService
    {
        public string Mode { get; set; } = "Light";

        public string Color { get; set; }

        public string ColorCssStyle => $"background-color:{Color}";

        public event Action ThemeChanged;

        public event Action ColorModeChanged;

        public void ChangeTheme(string color)
        {
            Color = color;
            ThemeChanged?.Invoke();
        }

        public void ToggleColorMode()
        {
            Mode = (Mode == "Dark" ? "Light" : "Dark");
            ColorModeChanged?.Invoke();
        }
    }
}


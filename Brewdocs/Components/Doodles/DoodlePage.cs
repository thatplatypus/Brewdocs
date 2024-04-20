using System;
using Brewdocs.Pages;
using Brewdocs.Shared;

namespace Brewdocs.Components.Doodles
{
    public class DoodlePage : PageBase
    {
        public override string Identifier => DocumentTypes.Doodles;

        public override string ThemeColor => ThemeColors.Doodles;

        public Doodle.Components.DoodleDraw DoodleDraw { get; set; } = default!;
    }
}


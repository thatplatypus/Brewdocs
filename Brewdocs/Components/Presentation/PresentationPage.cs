using System;
using Brewdocs.Pages;
using Brewdocs.Shared;

namespace Brewdocs.Components.Presentation
{
    public class PresentationPage : PageBase
    {
        public override string Identifier => DocumentTypes.Presentations;

        public override string ThemeColor => "var(--bs-orange)";
    }
}


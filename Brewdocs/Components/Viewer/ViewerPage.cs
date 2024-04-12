using System;
using Brewdocs.Pages;
using Brewdocs.Shared;

namespace Brewdocs.Components.Viewer
{
    public class ViewerPage : PageBase
    {
        public override string Identifier => DocumentTypes.Viewer;

        public override string ThemeColor => "var(--bs-gray)";

    }
}


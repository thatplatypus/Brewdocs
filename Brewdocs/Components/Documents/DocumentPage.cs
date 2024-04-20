using System;
using Brewdocs.Pages;
using Brewdocs.Shared;

namespace Brewdocs.Components.Documents
{
    public class DocumentPage : PageBase
    {
        public override string Identifier => DocumentTypes.Documents;

        public override string ThemeColor => ThemeColors.Documents;
    }
}


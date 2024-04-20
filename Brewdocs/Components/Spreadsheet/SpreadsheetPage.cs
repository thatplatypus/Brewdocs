using System;
using Brewdocs.Shared;

namespace Brewdocs.Components.Spreadsheet
{
    public class SpreadsheetPage : Pages.PageBase
    {
        public override string Identifier => DocumentTypes.Spreadsheets;

        public override string ThemeColor => ThemeColors.Spreadsheets;
    }
}


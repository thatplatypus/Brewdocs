using System;
namespace Brewdocs.Components.Spreadsheet
{
    public class CellViewModel
    {
        public string DisplayValue { get; set; }

        public string Value { get; set; }

        public bool IsFormula => Value.StartsWith('=');

        public string Cell { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Type DataType { get; set; } = typeof(string);
    }
}


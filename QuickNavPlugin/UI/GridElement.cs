using System.Collections.Generic;

namespace QuickNavPlugin.UI
{
    /// <summary>
    /// Represents a grid.
    /// </summary>
    public class GridElement : ContentElement
    {
        public int RowsColumns { get; set; }
        public Orientation Orientation { get; set; }
        public List<ContentElement> Children { get; } = new List<ContentElement>();

        public GridElement(int rowsColumns, Orientation orientation)
        {
            this.RowsColumns = rowsColumns;
            this.Orientation = orientation;
        }
    }
}

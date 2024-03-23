using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin.UI
{
    public delegate void ElementClicked(ContentElement sender);
    public delegate void ElementClickedPosition(ContentElement sender, double X, double Y);
    public delegate void ElementTextChanged(ContentElement sender, string text);
    public delegate void ElementImageChanged(ContentElement sender, Uri uri);
    public delegate void ElementIsEditableChanged(ContentElement sender, bool editable);
    public delegate void ElementScrollableChanged(ContentElement sender, bool scrollable);
    public delegate void ElementEqualSpacedChanged(ContentElement sender, bool equalspaced);
    public delegate void ElementProgressChanged(ContentElement sender, double progress);
    public delegate void ElementOrientationChanged(ContentElement sender, Orientation orientation);
    public delegate void ElementChildrenChanged(ContentElement sender, IEnumerable<ContentElement> children);
}

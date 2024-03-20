using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin.UI
{
    /// <summary>
    /// Superclass for plugin components. You should not derive from this.
    /// </summary>
    public class ContentElement
    {
        public ElementClicked Clicked { get; set; }
        public FlyoutElement Flyout { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin.UI
{
    /// <summary>
    /// Represents an image.
    /// </summary>
    public class ImageElement : ContentElement
    {
        private Uri _Image;
        public Uri Image
        {
            get => _Image;
            set
            {
                _Image = value;
                if(ImageChanged != null) ImageChanged(this, value);
            }
        }

        public ElementImageChanged ImageChanged;
        public ElementClickedPosition ClickedPosition;
    }
}

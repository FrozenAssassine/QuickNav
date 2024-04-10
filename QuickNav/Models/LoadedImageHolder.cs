using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.Models
{
    internal class LoadedImageHolder : Uri
    {
        public object Image = null;
        public object Type = ImageType.None;

        public LoadedImageHolder(Icon icon) : base("ms-appx://App/Assets/commands/launch.png")
        {
            Image = icon;
            Type = ImageType.Icon;
        }

        public LoadedImageHolder(Bitmap bitmap) : base("ms-appx://App/Assets/commands/launch.png")
        {
            Image = bitmap;
            Type = ImageType.Bitmap;
        }

        public LoadedImageHolder(ImageSource source) : base("ms-appx://App/Assets/commands/launch.png")
        {
            Image = source;
            Type = ImageType.ImageSource;
        }

        public enum ImageType
        {
            None,
            Icon,
            Bitmap,
            ImageSource
        }
    }
}

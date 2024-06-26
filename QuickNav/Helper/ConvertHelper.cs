using Microsoft.UI.Xaml;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.IO;
using Microsoft.UI.Xaml.Media;
using QuickNav.Models;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Windows.Media.Media3D;

namespace QuickNav.Helper;

internal class ConvertHelper
{
    public static Visibility BoolToVisibility(bool visible)
    {
        return visible ? Visibility.Visible : Visibility.Collapsed;
    }
    public static int ToInt(object value, int defaultValue = 0)
    {
        if (value != null)
        {
            if (int.TryParse(value.ToString(), out int converted) == true)
                return converted;
        }
        return defaultValue;
    }

    public static bool ToBoolean(object value, bool defaultValue = false)
    {
        if (value != null)
        {
            if (bool.TryParse(value.ToString(), out bool converted) == true)
                return converted;
        }
        return defaultValue;
    }

    //ref: https://stackoverflow.com/questions/76640972/convert-system-drawing-icon-to-microsoft-ui-xaml-imagesource
    public static async Task<SoftwareBitmapSource> GetWinUI3BitmapSourceFromIconAsync(System.Drawing.Icon icon)
    {
        if (icon == null)
            return null;

        using var bmp = icon.ToBitmap();
        return await GetWinUI3BitmapSourceFromGdiBitmapAsync(bmp);
    }
    public static async Task<SoftwareBitmapSource> GetWinUI3BitmapSourceFromGdiBitmapAsync(System.Drawing.Bitmap bmp)
    {
        if (bmp == null)
            return null;

        var data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
        var bytes = new byte[data.Stride * data.Height];
        Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
        bmp.UnlockBits(data);

        var softwareBitmap = new Windows.Graphics.Imaging.SoftwareBitmap(
            Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
            bmp.Width,
            bmp.Height,
            Windows.Graphics.Imaging.BitmapAlphaMode.Premultiplied);
        softwareBitmap.CopyFromBuffer(bytes.AsBuffer());

        var source = new Microsoft.UI.Xaml.Media.Imaging.SoftwareBitmapSource();
        await source.SetBitmapAsync(softwareBitmap);
        return source;
    }

    public static async Task<ImageSource> ConvertUriToImageSource(Uri uri)
    {
        if (uri == null)
            return null;
        if (uri is LoadedImageHolder lih)
        {
            switch (lih.Type)
            {
                case LoadedImageHolder.ImageType.None:
                    return null;
                case LoadedImageHolder.ImageType.Icon:
                    return await GetWinUI3BitmapSourceFromIconAsync((Icon)lih.Image);
                case LoadedImageHolder.ImageType.Bitmap:
                    return await GetWinUI3BitmapSourceFromGdiBitmapAsync((Bitmap)lih.Image);
                case LoadedImageHolder.ImageType.ImageSource:
                    return (ImageSource)lih.Image;
            }
            return null;
        }
        else return new BitmapImage(uri);
    }

    public static byte[] BitmapToIcon(Bitmap bmp) // https://gist.github.com/darkfall/1656050
    {
        if (bmp.Width > 256 || bmp.Height > 256)
        {
            int width = (int)((double)bmp.Width / Math.Max(bmp.Width, bmp.Height) * 256);
            int height = (int)((double)bmp.Height / Math.Max(bmp.Width, bmp.Height) * 256);
            bmp = new Bitmap(bmp, width, height);
        }

        MemoryStream mem_data = new MemoryStream();
        bmp.Save(mem_data, System.Drawing.Imaging.ImageFormat.Png);

        MemoryStream ms = new MemoryStream();
        BinaryWriter icon_writer = new BinaryWriter(ms);

        // 0-1 reserved, 0
        icon_writer.Write((byte)0);
        icon_writer.Write((byte)0);

        // 2-3 image type, 1 = icon, 2 = cursor
        icon_writer.Write((short)1);

        // 4-5 number of images
        icon_writer.Write((short)1);

        // image entry 1
        // 0 image width
        icon_writer.Write((byte)bmp.Width);
        // 1 image height
        icon_writer.Write((byte)bmp.Height);

        // 2 number of colors
        icon_writer.Write((byte)0);

        // 3 reserved
        icon_writer.Write((byte)0);

        // 4-5 color planes
        icon_writer.Write((short)0);

        // 6-7 bits per pixel
        icon_writer.Write((short)32);

        // 8-11 size of image data
        icon_writer.Write((int)mem_data.Length);

        // 12-15 offset of image data
        icon_writer.Write((int)(6 + 16));

        // write image data
        // png data must contain the whole png data file
        icon_writer.Write(mem_data.ToArray());

        icon_writer.Flush();
        icon_writer.Close();
        icon_writer.Dispose();

        return ms.ToArray();
    }
}

using Microsoft.UI.Xaml;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System;
using Microsoft.UI.Xaml.Media.Imaging;

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
}

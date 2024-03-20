using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace QuickNav.Views
{
    public sealed partial class ColorPicker : Page
    {
        public ColorPicker()
        {
            this.InitializeComponent();
        }

        public static (double h, double s, double v) ConvertToHSV(int red, int green, int blue)
        {
            // Normalize RGB values
            double r = red / 255.0;
            double g = green / 255.0;
            double b = blue / 255.0;

            // Find maximum and minimum values
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            // Calculate value (brightness)
            double value = max;

            // Calculate saturation
            double saturation = (max == 0) ? 0 : (max - min) / max;

            // Calculate hue
            double hue = 0;
            if (max != min)
            {
                double delta = max - min;
                if (max == r)
                {
                    hue = (g - b) / delta + (g < b ? 6 : 0);
                }
                else if (max == g)
                {
                    hue = (b - r) / delta + 2;
                }
                else // max == b
                {
                    hue = (r - g) / delta + 4;
                }

                hue *= 60;
            }

            return (hue, saturation * 100, value * 100);
        }


        private void CopyHSV_Click(object sender, RoutedEventArgs e)
        {
            var clr = colorPicker.Color;
            var hsv = ConvertToHSV(clr.R, clr.G, clr.B);
            if(clr.A == 255)
                System.Windows.Clipboard.SetText($"hsl({(int)hsv.h},{(int)hsv.s},{(int)hsv.v})");
            else
                System.Windows.Clipboard.SetText($"hsla({(int)hsv.h},{(int)hsv.s},{(int)hsv.v},{Math.Round(clr.A / 255.0, 3).ToString().Replace(",", ".")})");
        }

        private void CopyHEX_Click(object sender, RoutedEventArgs e)
        {
            var clr = colorPicker.Color;
            System.Windows.Clipboard.SetText("#" + (clr.A != 255 ? $"{clr.A:X2}" : "") + $"{clr.R:X2}{clr.G:X2}{clr.B:X2}");
        }

        private void CopyRGB_Click(object sender, RoutedEventArgs e)
        {
            var clr = colorPicker.Color;
            System.Windows.Clipboard.SetText($"rgb({clr.R},{clr.B},{clr.G}" + (clr.A != 255 ? "," + Math.Round(clr.A / 255.0, 3).ToString().Replace(",", ".") : "" + ")"));
        }
    }
}

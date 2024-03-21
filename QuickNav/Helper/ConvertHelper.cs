
using Microsoft.UI.Xaml;

namespace QuickNav.Helper;

internal class ConvertHelper
{
    public static Visibility BoolToVisibility(bool visible)
    {
        return visible ? Visibility.Visible : Visibility.Collapsed;
    }
}

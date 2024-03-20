using Microsoft.UI.Xaml.Controls;
using System;

namespace QuickNav.Extensions;

public static class MenuFlyoutExtension
{
    public static MenuFlyoutItem Add(this MenuFlyout menuFlyout, string text, string icon, object tag, Action<MenuFlyoutItem> clicked)
    {
        var menuItem = new MenuFlyoutItem
        {
            Text = text,
            Icon = new FontIcon { Glyph = icon },
            Tag = tag,
        };
        menuItem.Click += delegate
        {
            clicked(menuItem);
        };
        menuFlyout.Items.Add(menuItem);
        return menuItem;
    }
}

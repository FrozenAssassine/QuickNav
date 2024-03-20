using Microsoft.UI.Xaml.Controls;
using System;

namespace QuickNav.Extensions;

public static class MenuFlyoutExtension
{
    public static MenuFlyoutItem Add(this MenuFlyout menuFlyout, string text, string icon, Action clicked)
    {
        var menuItem = new MenuFlyoutItem
        {
            Text = text,
            Icon = new FontIcon { Glyph = icon },
        };
        menuItem.Click += delegate
        {
            clicked();
        };
        menuFlyout.Items.Add(menuItem);
        return menuItem;
    }
}

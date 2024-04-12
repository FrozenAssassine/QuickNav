using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using QuickNav.Views;
using System;
using System.IO;
using Windows.ApplicationModel;

namespace QuickNav.AppWindows;

public sealed partial class InfoWindow : Window
{
    public InfoWindow()
    {
        this.InitializeComponent();
        this.AppWindow.SetIcon(Path.Combine(Package.Current.InstalledLocation.Path, "Assets\\AppIcon\\appicon.ico"));
    }

    public void Select(Pages page)
    {
        if (page == Pages.Settings)
        {
            contentFrame.Navigate(typeof(SettingsPage));
            this.Title = "QuickNav Settings";
        }
        else if (page == Pages.About)
        {
            contentFrame.Navigate(typeof(AboutPage));
            this.Title = "About QuickNav";
        }
    }

    public enum Pages
    {
        Settings,
        About
    }

    private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        Select(Enum.Parse<Pages>(args.InvokedItem.ToString()));
    }
}

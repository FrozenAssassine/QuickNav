using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel;
using WinUIEx;

namespace QuickNav.AppWindows;

public sealed partial class SettingsWindow : Window
{
    public SettingsWindow(List<Window> openWindows)
    {
        this.InitializeComponent();
        this.Closed += delegate
        {
            openWindows.Remove(this);
        };
        this.SetWindowSize(600, 750);
        this.AppWindow.SetIcon(Path.Combine(Package.Current.InstalledLocation.Path, "Assets\\AppIcon\\appicon.ico"));
    }
}

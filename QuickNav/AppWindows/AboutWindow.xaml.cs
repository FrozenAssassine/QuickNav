using Microsoft.UI.Xaml;
using System.IO;
using Windows.ApplicationModel;

namespace QuickNav.AppWindows;

public sealed partial class AboutWindow : Window
{
    public AboutWindow()
    {
        this.InitializeComponent();

        this.AppWindow.SetIcon(Path.Combine(Package.Current.InstalledLocation.Path, "Assets\\AppIcon\\appicon.ico"));

    }
}

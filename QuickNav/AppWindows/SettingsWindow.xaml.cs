using Microsoft.UI.Xaml;
using QuickNav.BuildInCommands;
using QuickNav.Helper;
using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel;
using WinUIEx;

namespace QuickNav.AppWindows;

public sealed partial class SettingsWindow : Window
{
    const string AppName = "QuickNav";

    public SettingsWindow(List<Window> openWindows)
    {
        this.InitializeComponent();
        this.Closed += delegate
        {
            openWindows.Remove(this);
        };
        this.SetWindowSize(600, 750);
        this.AppWindow.SetIcon(Path.Combine(Package.Current.InstalledLocation.Path, "Assets\\AppIcon\\appicon.ico"));

        startupswitch.IsOn = StartupHelper.StartupExists(AppName);
        filesearchamount.Value = CommandSettings.AmountOfFiles;
        angleUnit.Items.Add("Radians");
        angleUnit.Items.Add("Degrees");
        angleUnit.SelectedIndex = CommandSettings.Radians ? 0 : 1;
        maxtayloriterations.Value = CommandSettings.MaxTaylorIterations;
    }

    private void filesearchamount_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args)
    {
        CommandSettings.AmountOfFiles = (int)filesearchamount.Value;
        CommandSettings.SaveAll();
    }

    private void maxtayloriterations_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args)
    {
        CommandSettings.MaxTaylorIterations = (int)maxtayloriterations.Value;
        CommandSettings.SaveAll();
    }

    private void angleUnit_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e)
    {
        CommandSettings.Radians = angleUnit.SelectedIndex == 0;
        CommandSettings.SaveAll();
    }

    private void startupswitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (startupswitch.IsOn)
        {
            StartupHelper.AddToStartup(AppName);
        }
        else
        {
            StartupHelper.RemoveFromStartup(AppName);
        }
    }
}

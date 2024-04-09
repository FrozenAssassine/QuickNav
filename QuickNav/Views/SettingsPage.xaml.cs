using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using QuickNav.BuildInCommands;
using QuickNav.Helper;

namespace QuickNav.Views
{
    public sealed partial class SettingsPage : Page
    {
        bool IsLoadingSettings = false;
        public SettingsPage()
        {
            this.InitializeComponent();
            LoadSettings();
        }

        private async void LoadSettings()
        {
            IsLoadingSettings = true;
            filesearchamount.Value = CommandSettings.AmountOfFiles;
            angleUnit.Items.Add("Radians");
            angleUnit.Items.Add("Degrees");
            angleUnit.SelectedIndex = CommandSettings.Radians ? 0 : 1;
            maxtayloriterations.Value = CommandSettings.MaxTaylorIterations;
            startupswitch.IsOn = await StartupHelper.StartupEnabled();

            IsLoadingSettings = false;
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

        private async void startupswitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (IsLoadingSettings)
                return;

            IsLoadingSettings = true;
            startupswitch.IsOn = await StartupHelper.ToggleLaunchOnStartup(startupswitch.IsOn);
            IsLoadingSettings = false;
        }
    }
}
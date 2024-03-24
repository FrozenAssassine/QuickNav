using Microsoft.UI.Xaml.Controls;
using QuickNav.Views.DialogViews;
using QuickNavPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;

namespace QuickNav.Dialogs
{
    internal class SetShortcutDialog
    {
        private SetShortcutDialogPage page = new SetShortcutDialogPage();
        public async Task<(VirtualKey[] keys, string query)> ShowAsync(ICommand command)
        {
            var dialog = new ContentDialog
            {
                Title = $"Shortcut for {command.Name("")}",
                PrimaryButtonText = "Done",
                CloseButtonText = "Cancel",
                SecondaryButtonText = "Reset",
                XamlRoot = MainWindow.mWindow.Content.XamlRoot,
                Content = page
            };
            dialog.Closing += (sender, args) =>
            {
                if (args.Result == ContentDialogResult.Secondary)
                {
                    page.pressedKeys.Clear();
                    page.UpdateShortcutDisplay();
                    args.Cancel = true;
                }
            };

            var dlgRes = await dialog.ShowAsync();
            if (dlgRes == ContentDialogResult.Primary)
            {
                dialog.Hide();
                return (page.pressedKeys.ToArray(), page.GetQuery);
            }
            return (null, null);
        }
    }
}

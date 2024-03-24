using Microsoft.UI.Xaml.Controls;
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
        private HashSet<VirtualKey> pressedKeys = new HashSet<VirtualKey>();

        private void UpdateShortcutDisplay(ContentDialog dialog)
        {
            dialog.Content = "Shortcut: ";
            foreach (var key in pressedKeys)
            {
                dialog.Content += key.ToString() + " ";
            }
        }

        public async Task<VirtualKey[]> ShowAsync(ICommand command)
        {
            var dialog = new ContentDialog
            {
                Title = $"Shortcut for {command.Name("")}",
                PrimaryButtonText = "Done",
                CloseButtonText = "Cancel",
                SecondaryButtonText = "Reset",
                XamlRoot = MainWindow.mWindow.Content.XamlRoot,
                Content = "Shortcut:"
            };
            dialog.Closing += (sender, args) =>
            {
                if (args.Result == ContentDialogResult.Secondary)
                {
                    pressedKeys.Clear();
                    UpdateShortcutDisplay(dialog);
                    args.Cancel = true;
                }
            };
            dialog.PreviewKeyDown += (sender, e) =>
            {
                pressedKeys.Add(e.Key);
                UpdateShortcutDisplay(dialog);
            };

            var dlgRes = await dialog.ShowAsync();
            if (dlgRes == ContentDialogResult.Primary)
            {
                dialog.Hide();
                return pressedKeys.ToArray();
            }
            return null;
        }
    }
}

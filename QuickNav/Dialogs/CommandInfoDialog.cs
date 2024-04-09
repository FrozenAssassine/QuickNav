using Microsoft.UI.Xaml.Controls;
using QuickNavPlugin;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuickNav.Dialogs;

internal class CommandInfoDialog
{
    public async Task ShowAsync(ICommand command)
    {
        var dialog = new ContentDialog
        {
            Title = $"About \"{command.Name("")}\"",
            PrimaryButtonText = "Done",
            XamlRoot = MainWindow.mWindow.Content.XamlRoot,
            Content = new TextBlock
            {
                Text = $"Trigger: \"{command.CommandTrigger}\"\n" +
                    $"Activation keywords: {string.Join(", ",  command.Keywords.Select(x => "\"" + x + "\""))}\n" + 
                    $"Description: \"{command.Description}\"",
                FontSize = 16, LineHeight = 28
            }
        };
        await dialog.ShowAsync();
    }
}

using Microsoft.UI.Xaml;
using QuickNav.Extensions;
using System;
using System.IO;
using Windows.ApplicationModel.DataTransfer;

namespace QuickNav.Widgets;

public sealed partial class CountWordsWidget : QuickNavWidget
{
    public CountWordsWidget()
    {
        this.InitializeComponent();
    }

    private async void Grid_Drop(object sender, DragEventArgs e)
    {
        if (e.DataView.Contains(StandardDataFormats.StorageItems))
        {
            var files = await e.DataView.GetStorageItemsAsync();
            if(files.Count > 0)
            {
                try
                {
                    var text = File.ReadAllText(files[0].Path);

                    infoDisplay.Text = "Words: " + text.CountWords() + "\nLines: " + text.CountLines() + "\nCharacters: " + text.Length;
                    infoDisplay.Visibility = Visibility.Visible;
                    infoText.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    //file error:
                    return;
                }
            }
        }
    }

    private void Grid_DragOver(object sender, DragEventArgs e)
    {
        e.AcceptedOperation = DataPackageOperation.Copy;
    }

    private void Grid_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
    {
        infoText.Visibility = Visibility.Collapsed;
    }
}

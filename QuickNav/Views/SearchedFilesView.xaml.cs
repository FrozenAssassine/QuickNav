using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using QuickNav.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace QuickNav.Views;

public sealed partial class SearchedFilesView : Page
{
    public SearchedFilesView()
    {
        this.InitializeComponent();
    }
    public async void ShowFiles(List<(string fileName, string filePath)> files)
    {
        foreach (var file in files)
        {
            ImageSource source = null;
            var icon = Win32Apis.GetIconForFile(file.filePath);
            if (icon != null)
                source = await ConvertHelper.GetWinUI3BitmapSourceFromIconAsync(icon);

            listView.Items.Add(new FilesViewItem { ImageSource = source, Name = file.fileName, Path = file.filePath });
        }
    }

    private void OpenFile(object item)
    {
        if (item is FilesViewItem filesViewItem)
        {
            string path = filesViewItem.Path;

            // Specify the process start information
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Path.GetFileName(path),
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(path),
            };

            try
            {
                Process.Start(startInfo);
            }
            catch { }
        }
    }

    private void listView_ItemClick(object sender, ItemClickEventArgs e)
    {
        OpenFile(e.ClickedItem);
    }

    private void CopyPath_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Clipboard.SetText(((sender as MenuFlyoutItem).Tag as FilesViewItem).Path);
    }

    private void CopyFileName_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Clipboard.SetText(((sender as MenuFlyoutItem).Tag as FilesViewItem).Name);
    }

    private void UserControl_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (listView.SelectedIndex == -1)
            listView.SelectedIndex = 0;

        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            OpenFile(listView.SelectedIndex);
        }
        else if (e.Key == Windows.System.VirtualKey.Down)
        {
            if (KeyHelper.IsKeyPressed(Windows.System.VirtualKey.Control))
                listView.SelectedIndex = listView.Items.Count - 1;
            else
                listView.SelectedIndex = Math.Clamp(listView.SelectedIndex + 1, 0, listView.Items.Count - 1);
        }
        else if (e.Key == Windows.System.VirtualKey.Up)
        {
            if (KeyHelper.IsKeyPressed(Windows.System.VirtualKey.Control))
                listView.SelectedIndex = 0;
            else
                listView.SelectedIndex = Math.Clamp(listView.SelectedIndex - 1, 0, listView.Items.Count - 1);
        }
    }

    private void ShowInExplorer_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        FileExplorerHelper.OpenExplorer(((sender as MenuFlyoutItem).Tag as FilesViewItem).Path);
    }

}
public class FilesViewItem
{
    public string Path { get; set; }
    public string Name { get; set; }
    public ImageSource ImageSource { get; set; }
}

using Microsoft.UI.Xaml.Controls;
using QuickNav.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QuickNav.Controls;

public sealed partial class SearchedFilesView : UserControl
{
    public SearchedFilesView()
    {
        this.InitializeComponent();
    }

    public void ShowFiles(List<(string fileName, string filePath)> files)
    {
        listView.ItemsSource = files.Select(file => new FilesViewItem { Name = file.fileName, Path = file.filePath });
    }

    private void listView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is FilesViewItem fVI)
            FileExplorerHelper.OpenExplorer(fVI.Path);
    }

    private void CopyPath_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Clipboard.SetText(((sender as MenuFlyoutItem).Tag as FilesViewItem).Path);
    }

    private void CopyFileName_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Clipboard.SetText(((sender as MenuFlyoutItem).Tag as FilesViewItem).Name);
    }
}
public class FilesViewItem
{
    public string Path { get; set; }
    public string Name { get; set; }
    public string IconSource { get; set; } 
}
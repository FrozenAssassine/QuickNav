using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using QuickNav.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

namespace QuickNav.Views
{
    public sealed partial class SearchedFilesView : Page
    {
        public SearchedFilesView()
        {
            this.InitializeComponent();
        }

        public void ShowFiles(List<(string fileName, string filePath)> files)
        {
            listView.ItemsSource = files.Select(file => new FilesViewItem { Name = file.fileName, Path = file.filePath });
        }

        private void OpenFile(object item)
        {
            if(item is FilesViewItem filesViewItem)
            {
                string path = filesViewItem.Path;

                // Specify the process start information
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = Path.GetFileName(path),
                    UseShellExecute = true,
                    WorkingDirectory = System.IO.Path.GetDirectoryName(path) + "\\"
                };

                // Start the process
                Process.Start(startInfo);
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
                listView.SelectedIndex = Math.Clamp(listView.SelectedIndex + 1, 0, listView.Items.Count - 1);
            else if (e.Key == Windows.System.VirtualKey.Up)
                listView.SelectedIndex = Math.Clamp(listView.SelectedIndex - 1, 0, listView.Items.Count - 1);
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
        public string IconSource { get; set; }
    }
}
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using QuickNav.BuildInCommands.ConverterCommand;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace QuickNav.Views
{
    public sealed partial class ConverterView : Page
    {
        readonly IConverter[] converters;
        readonly string[] files;
        string[] convertedFiles;

        public ConverterView(string[] files)
        {
            this.InitializeComponent();
            this.files = files;
            List<string> extensions = new List<string>();
            foreach (string file in files)
            {
                string ext = Path.GetExtension(file).ToLower().Replace(".", "");
                if (!extensions.Contains(ext))
                    extensions.Add(ext);
            }
            converters = ConverterRegistry.GetConverterFor(extensions.ToArray());
            List<string> usedExtensions = new List<string>();
            for(int i = 0; i < converters.Length; i++)
            {
                for(int j = 0; j < converters[i].OutputTypes.Length; j++)
                {
                    string ext = converters[i].OutputTypes[j];
                    if (!usedExtensions.Contains(ext))
                    {
                        listView.Items.Add(new ExtensionViewItem() { Extension = ext });
                        usedExtensions.Add(ext);
                    }
                }
            }
        }

        private void listView_ItemClick(object sender, ItemClickEventArgs e)
        {
            string ext = ((ExtensionViewItem)e.ClickedItem).Extension;
            foreach(IConverter conv in converters)
            {
                if (conv.OutputTypes.Contains(ext))
                {
                    convertedFiles = conv.ConvertTo(files, ext);
                    listView.Visibility = Visibility.Collapsed;
                    resultPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new();
            dataPackage.RequestedOperation = DataPackageOperation.Move;
            List<StorageFile> files = new List<StorageFile>();
            for(int i = 0; i < convertedFiles.Length; i++)
                files.Add(await StorageFile.GetFileFromPathAsync(convertedFiles[i]));
            dataPackage.SetStorageItems(files);
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
        }
    }

    public class ExtensionViewItem
    {
        public string Extension { get; set; }
    }
}

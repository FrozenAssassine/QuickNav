using Microsoft.UI.Xaml.Controls;
using Microsoft.WindowsAPICodePack.Shell;
using QuickNav.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace QuickNav.Views;

public sealed partial class SearchedAppsView : Page
{
    public SearchedAppsView()
    {
        this.InitializeComponent();
    }
    public void ShowApps(IEnumerable<ShellObject> apps)
    {
        //image icon: app.Thumbnail.ExtraLargeBitmapSource
        listView.ItemsSource = apps.Select(app => new SearchedAppItem { Name = app.Name, AppUserId = app.ParsingName});
    }

    public void LaunchApp(SearchedAppItem app)
    {
        LaunchApp(app.AppUserId);
    }
    public void LaunchApp(string userID)
    {
        Process.Start("explorer.exe", @" shell:appsFolder\" + userID);
    }
    private void listView_ItemClick(object sender, ItemClickEventArgs e)
    {
        LaunchApp(e.ClickedItem as SearchedAppItem);
    }

    private void Page_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (listView.SelectedIndex == -1)
            listView.SelectedIndex = 0;

        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            LaunchApp(listView.SelectedItem as SearchedAppItem);
        }
        else if (e.Key == Windows.System.VirtualKey.Down)
            listView.SelectedIndex = Math.Clamp(listView.SelectedIndex + 1, 0, listView.Items.Count - 1);
        else if (e.Key == Windows.System.VirtualKey.Up)
            listView.SelectedIndex = Math.Clamp(listView.SelectedIndex - 1, 0, listView.Items.Count - 1);
    }
}

public class SearchedAppItem
{
    public string AppUserId { get; set; }
    public string Name { get; set; }
}

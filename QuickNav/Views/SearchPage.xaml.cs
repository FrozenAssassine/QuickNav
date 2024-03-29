using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using QuickNav.Dialogs;
using QuickNav.Helper;
using QuickNav.Models;
using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

namespace QuickNav.Views;

public sealed partial class SearchPage : Page
{
    public bool PreventSearchboxChangedEvent = false;
    public bool ReloadDropList = true;

    public ICommand lastCommand = null;

    public SearchPage()
    {
        this.InitializeComponent();
        CommandShortcutHelper.Callback = RunCommand;
        CommandAutostartHelper.Callback = RunCommand;
    }

    public void InitialiseOnShowWindow()
    {
        searchBox.Text = "";
        searchBox.Focus(FocusState.Programmatic);
        searchInputBox_TextChanged(null, null);
        CommandAutostartHelper.RunCommands();
    }

    private void searchInputBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (PreventSearchboxChangedEvent)
        {
            PreventSearchboxChangedEvent = false;
            return;
        }

        if (lastCommand != null && lastCommand is IAbort)
            ((IAbort)lastCommand).Abort();

        //show items on hovering over
        contentView.Children.Clear();
        resultView.Items.Clear();
        contentView.Visibility = Visibility.Collapsed;
        resultView.Visibility = Visibility.Visible;

        WindowHelper.CenterWindow(MainWindow.hWnd);

        //home screen with nothing entered
        if (searchBox.Text.Length == 0)
        {
            var commands = PluginHelper.Plugins.Select(x => x.Commands);

            foreach (var commandList in commands)
            {
                foreach (var command in commandList)
                {
                    resultView.Items.Add(new ResultListViewItem() { Command = command, Text = command.Name(QueryHelper.FixQuery(command, searchBox.Text)) });
                }
            }
        }
        else
        {
            List<ICommand> commands = PluginHelper.SearchFor(searchBox.Text);
            List<ResultListViewItem> items = commands.Select((command) => new ResultListViewItem() { Command = command, Text = command.Name(QueryHelper.FixQuery(command, searchBox.Text)) }).ToList();
            for (int i = 0; i < items.Count; i++)
                resultView.Items.Add(items[i]);
        }

        ReloadDropList = true;
    }

    private void RunCommand(string query, ICommand command)
    {
        /*if (resultView.Items.Count == 0)
            return;*/

        MainWindow.mWindow.ShowAndFocus();

        if (query == null)
            query = "";
        
        if (lastCommand != null && lastCommand is IAbort)
            ((IAbort)lastCommand).Abort();

        if (command == null)
            return;

        lastCommand = command;

        query = QueryHelper.FixQuery(command, query);

        UIElement element = null;
        if (command is IBuildInCommand buildInCommand)
        {
            if (buildInCommand.RunCommand(query, out Page page, out double width, out double height))
            {
                element = page;
                WindowHelper.CenterWindow(MainWindow.hWnd, (int)width, (int)height);
            }
        }
        else
        {
            if (command.RunCommand(query, out ContentElement content))
                element = ContentElementRenderHelper.RenderContentElement(content);
        }
        if (element != null)
        {
            contentView.Children.Clear();
            contentView.Children.Add(element);
            resultView.Visibility = Visibility.Collapsed;
            contentView.Visibility = Visibility.Visible;
        }
        else
        {
            searchBox.Focus(FocusState.Keyboard);
        }

        ReloadDropList = true;
    }

    private void searchBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (resultView.SelectedIndex == -1)
            resultView.SelectedIndex = 0;

        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            //click on listviewitems
            if (resultView.Items.Count == 0)
                return;

            RunCommand(searchBox.Text, ((ResultListViewItem)resultView.Items[resultView.SelectedIndex]).Command);
        }
        else if (e.Key == Windows.System.VirtualKey.Down)
        {
            //a control is rendered
            if(contentView.Visibility == Visibility.Visible)
            {
                if(contentView.Children.Count > 0)
                    contentView.Children[0].Focus(FocusState.Programmatic);
                return;
            }

            resultView.SelectedIndex = Math.Clamp(resultView.SelectedIndex + 1, 0, resultView.Items.Count - 1);
        }
        else if (e.Key == Windows.System.VirtualKey.Up)
        {
            resultView.SelectedIndex = Math.Clamp(resultView.SelectedIndex - 1, 0, resultView.Items.Count - 1);
        }

        ReloadDropList = true;
    }

    private async void Grid_DragOver(object sender, DragEventArgs e)
    {
        e.AcceptedOperation = DataPackageOperation.Copy;

        if (e.DataView.Contains(StandardDataFormats.StorageItems))
        {
            if (!ReloadDropList)
                return;

            string extension = Path.GetExtension((await e.DataView.GetStorageItemsAsync())[0].Path).Substring(1).ToLower();

            List<IFileCommand> commands = PluginHelper.GetFilePlugins();
            resultView.Items.Clear();
            List<ResultListViewItem> items = commands
                .Where((IFileCommand cmd) => { return cmd.ExtensionFilter.Length == 0 || cmd.ExtensionFilter.Contains(extension); })
                .Select((command) => new ResultListViewItem() { Command = command, Text = command.Name(QueryHelper.FixQuery(command, searchBox.Text)) }).ToList();
            for (int i = 0; i < items.Count; i++)
                resultView.Items.Add(items[i]);

            ReloadDropList = false;
        }
    }

    private async void Grid_Drop(object sender, DragEventArgs e)
    {
        var position = e.GetPosition(resultView);
        int droppedItemIndex = -1;

        // Iterate through ListView items to find the item at the drop position
        for (int i = 0; i < resultView.Items.Count; i++)
        {
            ListViewItem item = resultView.ContainerFromIndex(i) as ListViewItem;
            if (item != null)
            {
                Rect itemBounds = item.TransformToVisual(resultView).TransformBounds(new Rect(0, 0, item.ActualWidth, item.ActualHeight));
                if (itemBounds.Contains(position))
                {
                    droppedItemIndex = i;
                    break;
                }
            }
        }

        //handle files:
        if (e.DataView.Contains(StandardDataFormats.StorageItems))
        {
            var files = await e.DataView.GetStorageItemsAsync();
            if (files.Count == 0)
                return;

            if (droppedItemIndex == -1)
            {
                searchBox.Text = files[0].Path;
            }

            var resultlistViewitem = resultView.Items[droppedItemIndex] as ResultListViewItem;
            PreventSearchboxChangedEvent = true;
            searchBox.Text = resultlistViewitem.Command.CommandTrigger + files[0].Path;
            RunCommand(resultlistViewitem.Command.CommandTrigger + files[0].Path, resultlistViewitem.Command);
        }
        //handle text:
        else if (e.DataView.Contains(StandardDataFormats.Text))
        {
            searchBox.Text += await e.DataView.GetTextAsync();
            var resultlistViewitem = resultView.Items[droppedItemIndex] as ResultListViewItem;
            PreventSearchboxChangedEvent = true;
            searchBox.Text = resultlistViewitem.Command.CommandTrigger + searchBox.Text;
            //RunCommand(resultlistViewitem.Command.CommandTrigger + searchBox.Text, resultlistViewitem);
        }

        ReloadDropList = true;
    }

    private void resultView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem == null)
            return;

        RunCommand(searchBox.Text, ((ResultListViewItem)e.ClickedItem).Command);
    }

    private async void searchBox_Drop(object sender, DragEventArgs e)
    {
        if (e.DataView.Contains(StandardDataFormats.StorageItems))
        {
            var files = await e.DataView.GetStorageItemsAsync();
            if (files.Count == 0)
                return;

            searchBox.Text += files[0].Path;
        }
        else if (e.DataView.Contains(StandardDataFormats.Text))
        {
            searchBox.Text += await e.DataView.GetTextAsync();
        }

        ReloadDropList = true;
    }

    private async void ConfigureListItemShortcut_Click(object sender, RoutedEventArgs e)
    {
        var clickedCommand = ((sender as MenuFlyoutItem).Tag as ResultListViewItem).Command;
        if (clickedCommand == null)
            return;

        var keys = await new SetShortcutDialog().ShowAsync(clickedCommand);
        if (keys.keys == null)
            return;

        CommandShortcutHelper.AddOrUpdate(keys.keys, keys.query, clickedCommand);

        searchInputBox_TextChanged(this, null);
    }

    private void RemoveShortcut_Click(object sender, RoutedEventArgs e)
    {
        var clickedCommand = ((sender as MenuFlyoutItem).Tag as ResultListViewItem).Command;
        if (clickedCommand == null)
            return;

        CommandShortcutHelper.RemoveShortcut(clickedCommand);

        searchInputBox_TextChanged(this, null);
    }

    private void LaunchOnReboot_Click(object sender, RoutedEventArgs e)
    {
        var clickedCommand = ((sender as MenuFlyoutItem).Tag as ResultListViewItem).Command;
        if (clickedCommand == null)
            return;

        CommandAutostartHelper.AddCommand(clickedCommand, "");

        searchInputBox_TextChanged(this, null);
    }

    private void RemoveFromReboot_Click(object sender, RoutedEventArgs e)
    {
        var clickedCommand = ((sender as MenuFlyoutItem).Tag as ResultListViewItem).Command;
        if (clickedCommand == null)
            return;

        CommandAutostartHelper.RemoveCommand(clickedCommand);

        searchInputBox_TextChanged(this, null);
    }
}

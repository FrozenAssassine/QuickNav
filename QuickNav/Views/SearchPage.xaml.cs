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
using System.Threading.Tasks;
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

    private async Task AddCommands(List<ICommand> commands)
    {
        await AddCommands(commands);
    }
    private async Task AddCommands(IEnumerable<ICommand> commands)
    {
        foreach (var command in commands)
        {
            string fixedQuery = QueryHelper.FixQuery(command, searchBox.Text);
            var uri = await ConvertHelper.ConvertUriToImageSource(command.Icon(fixedQuery));
            resultView.Items.Add(new ResultListViewItem() { Command = command, Text = command.Name(fixedQuery), Uri = uri });
        }
    }

    private async void searchInputBox_TextChanged(object sender, TextChangedEventArgs e)
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

        resultView.Items.Clear();

        IEnumerable<ICommand> commands;
        if (searchBox.Text.Length == 0) //home screen with nothing entered
            commands = PluginHelper.Plugins.SelectMany(x => x.Commands);
        else //search page
            commands = PluginHelper.SearchFor(searchBox.Text);

        await AddCommands(commands);

        ReloadDropList = true;
    }

    private void RunCommand(string query, ICommand command)
    {
        if (command == null)
            return;

        if (query == null)
            query = "";
        
        if (lastCommand != null && lastCommand is IAbort)
            ((IAbort)lastCommand).Abort();

        
        lastCommand = command;
        query = QueryHelper.FixQuery(command, query);
        UIElement element = null;
        
        MainWindow.mWindow.ShowAndFocus();

        if (command is IBuildInCommand buildInCommand)
        {
            if (buildInCommand.RunCommand(query, out Page page, out double width, out double height))
            {
                element = page;
                WindowHelper.CenterWindow(MainWindow.hWnd, (int)width, (int)height);
            }
            else if (buildInCommand.CommandTrigger.Length != 0)
            {
                searchBox.Text = buildInCommand.CommandTrigger;
                searchBox.SelectionStart = searchBox.Text.Length;
            }
        }
        else
        {
            if (command.RunCommand(query, out ContentElement content))
                element = ContentElementRenderHelper.RenderContentElement(content);
            else if (command.CommandTrigger.Length != 0)
            {
                searchBox.Text = command.CommandTrigger;
                searchBox.SelectionStart = searchBox.Text.Length;
            }
        }

        if (element != null)
        {
            contentView.Children.Clear();
            contentView.Children.Add(element);
            resultView.Visibility = Visibility.Collapsed;
            contentView.Visibility = Visibility.Visible;
        }
        else
            searchBox.Focus(FocusState.Keyboard);

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

            if (KeyHelper.IsKeyPressed(Windows.System.VirtualKey.Control))
                resultView.SelectedIndex = resultView.Items.Count -1;
            else
                resultView.SelectedIndex = Math.Clamp(resultView.SelectedIndex + 1, 0, resultView.Items.Count - 1);
        }
        else if (e.Key == Windows.System.VirtualKey.Up)
        {
            if (KeyHelper.IsKeyPressed(Windows.System.VirtualKey.Control))
                resultView.SelectedIndex = 0;
            else
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

            string extension = Path.GetExtension((await e.DataView.GetStorageItemsAsync())[0].Path).ToLower();
            if (extension.Length > 0)
                extension = extension.Substring(1);

            List<IFileCommand> commands = PluginHelper.GetFilePlugins();
            resultView.Items.Clear();
            List<IFileCommand> fitems = commands
                .Where((IFileCommand cmd) => { return cmd.ExtensionFilter.Length == 0 || cmd.ExtensionFilter.Contains(extension); }).ToList();

            await AddCommands(fitems);

            ReloadDropList = false;
        }
        else if (e.DataView.Contains(StandardDataFormats.Text)) 
        {
            if (!ReloadDropList)
                return;

            List<ITextCommand> commands = PluginHelper.GetTextPlugins();
            resultView.Items.Clear();
            await AddCommands(commands);

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
            string text = await e.DataView.GetTextAsync();
            var resultlistViewitem = resultView.Items[droppedItemIndex] as ResultListViewItem;
            PreventSearchboxChangedEvent = true;
            searchBox.Text = resultlistViewitem.Command.CommandTrigger;
            RunCommand(resultlistViewitem.Command.CommandTrigger + text, resultlistViewitem.Command);
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

    private async void OpenCommandInfo_Click(object sender, RoutedEventArgs e)
    {
        var clickedCommand = ((sender as MenuFlyoutItem).Tag as ResultListViewItem).Command;
        if (clickedCommand == null)
            return;

        await new CommandInfoDialog().ShowAsync(clickedCommand);
    }
}

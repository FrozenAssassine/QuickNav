using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using QuickNav.BuildInCommands;
using QuickNav.Helper;
using QuickNav.Models;
using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using WinRT.Interop;
using System.Windows.Threading;
using Microsoft.UI.Dispatching;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using WinUIEx;
using H.NotifyIcon.Core;

//ref systray to:
//https://github.com/dotMorten/WinUIEx/blob/3aaa34b58c488dba054e80311bfc3aa1cc772950/src/WinUIExSample/MainWindow.xaml.cs
//https://github.com/dotMorten/WinUIEx/blob/3aaa34b58c488dba054e80311bfc3aa1cc772950/src/WinUIExSample/MainWindow.xaml

namespace QuickNav
{
    public sealed partial class MainWindow : WindowEx
    {
        public static AppWindow m_AppWindow;
        private OverlappedPresenter? _presenter;
        public static DispatcherQueue dispatcherQueue;
        public bool PreventSearchboxChangedEvent = false;
        private TrayIcon trayIcon;

        public MainWindow()
        {
            this.InitializeComponent();

            m_AppWindow = GetAppWindowForCurrentWindow();

            this.Activated += MainWindow_Activated;
            dispatcherQueue = this.DispatcherQueue;
            _presenter = m_AppWindow.Presenter as OverlappedPresenter;

            BuildInCommandRegistry.Register();

            searchInputBox_TextChanged(null, null);

            InitSystemTray();
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        private void InitSystemTray()
        {
            if(trayIcon is null)
            {
                //var icon = Icon.FromFile("Images/WindowIcon.ico");
                trayIcon = new TrayIcon("QuickNav");
            }
            else
            {
                trayIcon.Dispose();
                trayIcon = null;
            }
        }

        private void HideWindow()
        {
            //trayIcon?.Dispose();
            //var icon = Icon.FromFile("Images/WindowIcon.ico");
            //trayIcon = new TrayIcon("QuickNav");
            this.HideWindow();
        }

        private async void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.Deactivated)
            {
                HideWindow();
                return;
            }

            if (_presenter is null)
            {
                return;
            }

            await WindowHelper.CenterWindow(m_AppWindow);

            _presenter.SetBorderAndTitleBar(hasBorder: false, hasTitleBar: false);
            _presenter.IsAlwaysOnTop = true;
            _presenter.IsResizable = false;

            searchBox.Focus(FocusState.Keyboard);
        }

        private void searchInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PreventSearchboxChangedEvent)
            {
                PreventSearchboxChangedEvent = false;
                return;
            }

            //home screen with nothing entered
            if (searchBox.Text.Length == 0)
            {
                //show items on hovering over
                contentView.Children.Clear();
                resultView.Items.Clear();
                contentView.Visibility = Visibility.Collapsed;
                resultView.Visibility = Visibility.Visible;

                //Todo simpler approach to add the items
                var commands = PluginHelper.Plugins.Select(x => x.CollectorCommands);
                var triggerCommands = PluginHelper.Plugins.Select(x => x.TriggerCommands);

                foreach (var commandList in triggerCommands)
                {
                    foreach (var command in commandList)
                    {
                        resultView.Items.Add(new ResultListViewItem() { Command = command, Text = command.Name(searchBox.Text) });
                    }
                }
                return;
            }
            else
            {
                contentView.Children.Clear();
                resultView.Items.Clear();
                contentView.Visibility = Visibility.Collapsed;
                resultView.Visibility = Visibility.Visible;
                if (searchBox.Text == "") return;
                List<ICommand> commands = PluginHelper.SearchFor(searchBox.Text);
                List<ResultListViewItem> items = commands.Select((command) => new ResultListViewItem() { Command = command, Text = command.Name(searchBox.Text) }).ToList();
                for (int i = 0; i < items.Count; i++)
                    resultView.Items.Add(items[i]);
            }
        }

        private void RunCommand(string query, ResultListViewItem item)
        {
            if (resultView.Items.Count == 0 || query.Length == 0)
                return;

            var command = item.Command;
            if (command == null) 
                return;

            query = query.Trim();

            if (command is ITriggerCommand trigger)
                query = query.Substring(trigger.CommandTrigger.Length).TrimStart();

            UIElement element = null;
            if (command is IBuildInCommand buildInCommand)
            {
                if (buildInCommand.RunCommand(query, out Page page))
                    element = page;
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
                //command was executed without showing ui
                //this.Close();
            }
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

                RunCommand(searchBox.Text, (ResultListViewItem)resultView.Items[resultView.SelectedIndex]);
            }
            else if(e.Key == Windows.System.VirtualKey.Down)
            {
                resultView.SelectedIndex = Math.Clamp(resultView.SelectedIndex + 1, 0, resultView.Items.Count - 1);
            }
            else if (e.Key == Windows.System.VirtualKey.Up)
            {
                resultView.SelectedIndex = Math.Clamp(resultView.SelectedIndex - 1, 0, resultView.Items.Count - 1);
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
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
                if (resultlistViewitem.Command is ITriggerCommand triggerCommand)
                {
                    PreventSearchboxChangedEvent = true;
                    searchBox.Text = triggerCommand.CommandTrigger + files[0].Path;
                    RunCommand(triggerCommand.CommandTrigger + files[0].Path, resultlistViewitem);
                }
                else if (resultlistViewitem.Command is IUnknownCommandCollector collector)
                {
                    //any ideas?
                }
            }
        }

        private void resultView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //click on listviewitems
            if (resultView.Items.Count == 0)
                return;

            RunCommand(searchBox.Text, (ResultListViewItem)resultView.Items[resultView.SelectedIndex == -1 ? 0 : resultView.SelectedIndex]);
        }
    }
}

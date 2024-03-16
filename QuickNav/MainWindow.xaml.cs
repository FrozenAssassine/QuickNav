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

namespace QuickNav
{
    public sealed partial class MainWindow : Window
    {
        public static AppWindow m_AppWindow;
        private OverlappedPresenter? _presenter;
        public static DispatcherQueue dispatcherQueue;

        public MainWindow()
        {
            this.InitializeComponent();

            m_AppWindow = GetAppWindowForCurrentWindow();

            this.Activated += MainWindow_Activated;
            dispatcherQueue = this.DispatcherQueue;
            _presenter = m_AppWindow.Presenter as OverlappedPresenter;

            //hide the window from taskbar and ALT+Tab:
            this.AppWindow.IsShownInSwitchers = false;

            BuildInCommandRegistry.Register();
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        private async void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.Deactivated)
            {
                //close the window when it loses focus:
                //this.Close();
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

        private void resultView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultView.Items.Count == 0)
                return;

            ICommand command = ((ResultListViewItem)resultView.Items[resultView.SelectedIndex]).Command;
            if (command == null) return;
            string searchText = searchBox.Text.Trim();
            if (command is ITriggerCommand trigger)
                searchText = searchText.Substring(trigger.CommandTrigger.Length).TrimStart();
            UIElement element = null;
            if (command is IBuildInCommand buildInCommand)
            {
                if (buildInCommand.RunCommand(searchText, out Page page))
                    element = page;
            }
            else
            {
                if (command.RunCommand(searchText, out ContentElement content))
                    element = ContentElementRenderHelper.RenderContentElement(content);
            }
            if (element != null)
            {
                contentView.Children.Clear();
                contentView.Children.Add(element);
                resultView.Visibility = Visibility.Collapsed;
                contentView.Visibility = Visibility.Visible;
            }
        }

        private void searchBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                resultView.SelectedIndex = 0;
            }
        }
    }
}

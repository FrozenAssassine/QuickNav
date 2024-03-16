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

namespace QuickNav
{
    public sealed partial class MainWindow : Window
    {
        private AppWindow m_AppWindow;
        private OverlappedPresenter? _presenter;

        public MainWindow()
        {
            this.InitializeComponent();

            m_AppWindow = GetAppWindowForCurrentWindow();

            this.Activated += MainWindow_Activated;

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

            await WindowHelper.CenterWindow(this.m_AppWindow);

            _presenter.SetBorderAndTitleBar(hasBorder: false, hasTitleBar: false);
            _presenter.IsAlwaysOnTop = true;
            _presenter.IsResizable = false;
        }

        private void searchInputBox_TextChanged(object sender, Microsoft.UI.Xaml.Controls.TextChangedEventArgs e)
        {
            List<ICommand> commands = PluginHelper.SearchFor(searchBox.Text);
            List<ResultListViewItem> items = commands.Select((command) => new ResultListViewItem() { Command = command, Text = command.Name(searchBox.Text) }).ToList();
            resultView.Items.Clear();
            for (int i = 0; i < items.Count; i++)
                resultView.Items.Add(items[i]);
            contentView.Children.Clear();
            contentView.Visibility = Visibility.Collapsed;
            resultView.Visibility = Visibility.Visible;
        }

        private void resultView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultView.Items.Count == 0)
                return;

            ICommand command = ((ResultListViewItem)resultView.Items[resultView.SelectedIndex]).Command;
            UIElement element = null;
            if (command is IBuildInCommand buildInCommand)
            {
                if (buildInCommand.RunCommand(searchBox.Text, out Page page))
                    element = page;
            }
            else
            {
                if (command.RunCommand(searchBox.Text, out ContentElement content))
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
    }
}

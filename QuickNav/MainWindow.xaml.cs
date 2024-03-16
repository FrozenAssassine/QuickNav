using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using QuickNav.BuildInCommands;
using QuickNav.Helper;
using System;
using System.Threading.Tasks;
using Windows.Devices.Display;
using Windows.Devices.Enumeration;
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
            if (_presenter is null)
            {
                return;
            }

            await WindowHelper.CenterWindow(this.m_AppWindow);

            _presenter.SetBorderAndTitleBar(hasBorder: false, hasTitleBar: false);
            _presenter.IsAlwaysOnTop = true;
            _presenter.IsResizable = false;
        }
    }
}

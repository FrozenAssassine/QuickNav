using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using QuickNav.Helper;
using System;
using WinRT.Interop;
using Microsoft.UI.Dispatching;
using QuickNav.BuildInCommands;
using H.NotifyIcon;

namespace QuickNav;

public sealed partial class MainWindow : Window
{
    public static AppWindow m_AppWindow;
    public static IntPtr hWnd;
    private OverlappedPresenter? _presenter;
    public static DispatcherQueue dispatcherQueue;

    public MainWindow()
    {
        this.InitializeComponent();
        this.Hide();
        hWnd = WindowNative.GetWindowHandle(this);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);

        m_AppWindow = AppWindow.GetFromWindowId(wndId);

        this.Activated += MainWindow_Activated;
            dispatcherQueue = this.DispatcherQueue;
        _presenter = m_AppWindow.Presenter as OverlappedPresenter;

        this.AppWindow.IsShownInSwitchers = false;

        BuildInCommandRegistry.Register();

        GlobalHotkeyHelper.RegisterHotkey(Windows.System.VirtualKeyModifiers.Windows, Windows.System.VirtualKey.Y, (object sender, EventArgs e) =>
        {
            this.Show();
        });

        if (_presenter is null)
        {
            return;
        }

        _presenter.SetBorderAndTitleBar(hasBorder: false, hasTitleBar: false);
        _presenter.IsAlwaysOnTop = true;
        _presenter.IsResizable = false;
    }
    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        if (args.WindowActivationState == WindowActivationState.Deactivated)
        {
            m_AppWindow.Hide();
            return;
        }

        WindowHelper.CenterWindow(hWnd);
        searchPage.InitialiseOnShowWindow();
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        GlobalHotkeyHelper.UnregisterAllHotkeys();
    }
}

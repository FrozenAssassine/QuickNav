using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using QuickNav.Helper;
using System;
using WinRT.Interop;
using Microsoft.UI.Dispatching;
using QuickNav.BuildInCommands;
using H.NotifyIcon;
using Microsoft.UI.Xaml.Input;
using System.Runtime.InteropServices;
using System.IO;
using Windows.ApplicationModel;
using H.NotifyIcon.Core;
using WinUIEx;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing;
using QuickNav.AppWindows;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Linq;

namespace QuickNav;

public sealed partial class MainWindow : Window
{
    public static AppWindow m_AppWindow;
    public static IntPtr hWnd;
    private OverlappedPresenter? _presenter;
    public static DispatcherQueue dispatcherQueue;
    public static MainWindow mWindow;

    public MainWindow()
    {
        this.InitializeComponent();
        this.Hide();
        mWindow = this;

        hWnd = WindowNative.GetWindowHandle(this);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);

        m_AppWindow = AppWindow.GetFromWindowId(wndId);

        this.Activated += MainWindow_Activated;
            dispatcherQueue = this.DispatcherQueue;
        _presenter = m_AppWindow.Presenter as OverlappedPresenter;

        this.AppWindow.SetIcon(Path.Combine(Package.Current.InstalledLocation.Path, "Assets\\AppIcon\\appicon.ico"));
        systrayHandle.Icon = new System.Drawing.Icon(Path.Combine(Package.Current.InstalledLocation.Path, "Assets\\AppIcon\\appicon.ico"));

        CommandSettings.LoadAll();
        CommandShortcutHelper.GetShortcuts();
        CommandShortcutHelper.RegisterAll();
        BuildInCommandRegistry.Register();

        GlobalHotkeyHelper.RegisterHotkey(Windows.System.VirtualKeyModifiers.Windows, Windows.System.VirtualKey.Y, (object sender, EventArgs e) =>
        {
            ShowAndFocus();
        }, out int x);

        if (_presenter is null)
        {
            return;
        }
        _presenter.SetBorderAndTitleBar(hasBorder: false, hasTitleBar: false);
        _presenter.IsAlwaysOnTop = true;
        _presenter.IsResizable = false;
        this.AppWindow.IsShownInSwitchers = false;
    }

    public void ShowAndFocus()
    {
        this.Show();
        Win32Apis.SetForegroundWindow(hWnd);
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
        CommandSettings.SaveAll();
        GlobalHotkeyHelper.UnregisterAllHotkeys();
    }

    private void searchPage_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if(e.Key == Windows.System.VirtualKey.Escape)
        {
            this.Hide();
        }
    }

    private void ShowSettings_Click(object sender, RoutedEventArgs e)
    {
        ShowWindowHelper.ShowWindow<SettingsWindow>();
    }

    private void ShowAbout_Click(object sender, RoutedEventArgs e)
    {
        ShowWindowHelper.ShowWindow<AboutWindow>();
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        ShowWindowHelper.CloseAll();
        this.Close();
        systrayHandle.Dispose();
    }
}

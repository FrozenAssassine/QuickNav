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

namespace QuickNav;

public sealed partial class MainWindow : Window
{
    public static AppWindow m_AppWindow;
    public static IntPtr hWnd;
    private readonly OverlappedPresenter? _presenter;
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

        Title = Package.Current.DisplayName;
        this.AppWindow.SetIcon(Path.Combine(Package.Current.InstalledLocation.Path, "Assets\\AppIcon\\appicon.ico"));
        systrayHandle.Icon = new System.Drawing.Icon(Path.Combine(Package.Current.InstalledLocation.Path, "Assets\\AppIcon\\appicon.ico"));

        BuildInCommandRegistry.Register();

        string extDir = Path.Combine(Package.Current.InstalledLocation.Path, "Extensions");
        if (Directory.Exists(extDir))
            PluginHelper.InitFromDir(extDir);

        CommandSettings.LoadAll();
        CommandShortcutHelper.GetShortcuts();
        CommandShortcutHelper.RegisterAll();
        CommandAutostartHelper.LoadAll();

        GlobalHotkeyHelper.RegisterHotkey(Windows.System.VirtualKeyModifiers.Windows, Windows.System.VirtualKey.Y, (object sender, EventArgs e) =>
        {
            ShowWindow();
        }, out int x);

        if (_presenter is null)
        {
            return;
        }
        _presenter.SetBorderAndTitleBar(hasBorder: false, hasTitleBar: false);
        _presenter.IsAlwaysOnTop = true;
        _presenter.IsResizable = false;
        this.AppWindow.IsShownInSwitchers = false;

        if (CommandAutostartHelper.AutostartCommands.Count > 0)
            ShowAndFocus();
    }

    private void ShowWindow()
    {
        for (int i = 0; i < PluginHelper.Plugins.Count; i++)
            for (int j = 0; j < PluginHelper.Plugins[i].Commands.Count; j++)
                PluginHelper.Plugins[i].Commands[j].OnWindowOpened();
        ShowAndFocus();
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

    private void SearchPage_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if(e.Key == Windows.System.VirtualKey.Escape)
        {
            this.Hide();
        }
    }
    
    private void ShowSettings_Click(object sender, RoutedEventArgs e)
    {
        ShowWindowHelper.ShowInfoWindow(InfoWindow.Pages.Settings);
    }

    private void ShowAbout_Click(object sender, RoutedEventArgs e)
    {
        ShowWindowHelper.ShowInfoWindow(InfoWindow.Pages.About);
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        ShowWindowHelper.CloseAll();
        this.Close();
        systrayHandle.Dispose();
    }

    private void ShowWindow_Click(object sender, RoutedEventArgs e)
    {
        ShowWindow();
    }
}

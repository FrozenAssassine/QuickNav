using H.NotifyIcon;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using QuickNav.Helper;
using System;
using System.Runtime.InteropServices;
using WinRT.Interop;
using WinUIEx;

namespace QuickNav.Widgets
{
    public class QuickNavWidget : WindowEx
    {
        private IntPtr hWnd;
        public string WidgetName { get; set; }

    // Helper class for Win32 interop
    public const int WS_EX_TOOLWINDOW = 0x00000080;
    public const int WS_EX_NOACTIVATE = 0x08000000;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    public QuickNavWidget()
        {
            this.Title = WidgetName;
            this.IsShownInSwitchers = false;
            IsMaximizable = IsMinimizable = false;

            hWnd = WindowNative.GetWindowHandle(this);
            this.SetWindowSize(400, 300);
            ExtendsContentIntoTitleBar = true;

            this.SystemBackdrop = new TransparentTintBackdrop();

            int exStyle = GetWindowLong(hWnd, -20); // GWL_EXSTYLE
            exStyle |= WS_EX_TOOLWINDOW;
            SetWindowLong(hWnd, -20, exStyle); // GWL_EXSTYLE
        }
    }
}
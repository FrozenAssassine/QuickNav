using System;
using System.Runtime.InteropServices;

namespace QuickNav.Helper;

internal class Win32Apis
{
    //Lock your device:
    [DllImport("user32.dll")]
    public static extern bool LockWorkStation();

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hwnd, uint msg, int wParam, int lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
}

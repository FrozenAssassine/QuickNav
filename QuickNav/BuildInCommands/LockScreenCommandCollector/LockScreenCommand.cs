using QuickNavPlugin;
using System;
using QuickNav.Helper;
using System.Threading;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace QuickNav.BuildInCommands.LockScreenCommandCollector;

internal class LockScreenCommand : ICommand
{
    const int SYSCOM = 0x0112;
    const int MONPOW = 0xF170;
    const int MON_OFF = 2;

    public string Description => "Run this to lock your screen";

    public Uri Icon(string query) => new Uri("ms-appx://App/Assets/commands/lockscreen.png");

    public Priority Priority(string query)
    {
        return QuickNavPlugin.Priority.Low;
    }

    public string CommandTrigger => "lock";

    public string[] Keywords => new string[] { "lock", "screen"};

    public string Name(string query)
    {
        return "Lock your screen";
    }

    public bool RunCommand(string searchTerm, out QuickNavPlugin.UI.ContentElement content)
    {
        content = null;

        Win32Apis.LockWorkStation();
        Thread.Sleep(2000); // Wait 2 sec otherwise some background tasks from LockWorkStation() will turn the screen on again
        IntPtr handle = Win32Apis.FindWindow(null, null);
        Win32Apis.SendMessage(handle, SYSCOM, MONPOW, MON_OFF);

        return true;
    }

    public void OnWindowOpened() { }
}
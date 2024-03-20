using System.Runtime.InteropServices;

namespace QuickNav.Helper;

internal class Win32Apis
{
    //Lock your device:
    [DllImport("user32.dll")]
    public static extern bool LockWorkStation();
}

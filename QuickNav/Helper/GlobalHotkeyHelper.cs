using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Windows.System;
using WinRT.Interop;

namespace QuickNav.Helper
{
    internal static class GlobalHotkeyHelper
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")] //32-bit
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        public delegate IntPtr WndProcDelegate(IntPtr hwnd, uint message, IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);
        
        private static IntPtr _oldWndProc = IntPtr.Zero;

        private static int HOTKEY_ID = 9001;
        private const int WM_HOTKEY = 0x0312;
        private const int GWLP_WNDPROC = -4;

        private static List<(int HotKeyID, VirtualKeyModifiers Modifier, VirtualKey Key, EventHandler Event)> hotkeyRegistry = new List<(int, VirtualKeyModifiers, VirtualKey, EventHandler)>();

        public static bool RegisterHotkey(VirtualKeyModifiers modifier, VirtualKey key, EventHandler hotkeyPressed)
        {
            if (_oldWndProc == IntPtr.Zero)
                _oldWndProc = SetWndProc(HwndHook);
            
            bool success = RegisterHotKey(MainWindow.WindowHandle, HOTKEY_ID, (uint)modifier, (uint)key);
            if (success)
                hotkeyRegistry.Add((HOTKEY_ID++, modifier, key, hotkeyPressed));
            return success;
        }

        public static void UnregisterAllHotkeys()
        {
            for(int i = 0; i < hotkeyRegistry.Count; i++)
            {
                UnregisterHotKey(MainWindow.WindowHandle, hotkeyRegistry[i].HotKeyID);
            }
        }

        public static IntPtr SetWndProc(WndProcDelegate newProc)
        {
            IntPtr functionPointer = Marshal.GetFunctionPointerForDelegate(newProc);
            if (IntPtr.Size == 8)
                return SetWindowLongPtr(MainWindow.WindowHandle, GWLP_WNDPROC, functionPointer);
            else
                return SetWindowLong(MainWindow.WindowHandle, GWLP_WNDPROC, functionPointer);
        }

        private static IntPtr HwndHook(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if(msg == WM_HOTKEY)
                for (int i = 0; i < hotkeyRegistry.Count; i++)
                    if (hotkeyRegistry[i].HotKeyID == wParam.ToInt32() && hotkeyRegistry[i].Event != null)
                        hotkeyRegistry[i].Event(null, EventArgs.Empty);
            return CallWindowProc(_oldWndProc, hwnd, msg, wParam, lParam);
        }
    }
}

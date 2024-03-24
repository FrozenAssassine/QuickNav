using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Windows.System;

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
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);
        
        private static IntPtr _oldWndProc = IntPtr.Zero;
        public delegate IntPtr WndProcDelegate(IntPtr hwnd, uint message, IntPtr wParam, IntPtr lParam);
        private static WndProcDelegate _wndProcDelegate;

        private static int HOTKEY_ID = 9001;
        private const int WM_HOTKEY = 0x0312;
        private const int GWLP_WNDPROC = -4;

        private static List<(int HotKeyID, EventHandler Event, object obj)> hotkeyRegistry = new List<(int, EventHandler, object)>();

        private static uint GetModifier(VirtualKey[] keys)
        {
            uint modifier = 0;
            foreach (var key in keys)
            {
                if (key == VirtualKey.Control || key == VirtualKey.LeftControl || key == VirtualKey.RightControl)
                {
                    modifier |= 0x2; // CTRL
                }
                else if (key == VirtualKey.Shift || key == VirtualKey.LeftShift || key == VirtualKey.RightShift)
                {
                    modifier |= 0x4; // SHIFT
                }
                else if (key == VirtualKey.Menu || key == VirtualKey.LeftMenu || key == VirtualKey.RightMenu)
                {
                    modifier |= 0x1; // ALT
                }
                else if (key == VirtualKey.LeftWindows || key == VirtualKey.RightWindows)
                {
                    modifier |= 0x8; // Windows key
                }
            }
            return modifier;
        }

        private static bool IsModifierKey(VirtualKey key)
        {
            return key == VirtualKey.Control || key == VirtualKey.LeftControl || key == VirtualKey.RightControl ||
                   key == VirtualKey.Shift || key == VirtualKey.LeftShift || key == VirtualKey.RightShift ||
                   key == VirtualKey.RightWindows|| key == VirtualKey.LeftWindows ||
                   key == VirtualKey.Menu || key == VirtualKey.LeftMenu || key == VirtualKey.RightMenu;
        }
        public static bool RegisterHotkey(VirtualKey[] keys, EventHandler hotkeyPressed, out int HotkeyID, object obj = null)
        {
            if (_oldWndProc == IntPtr.Zero)
            {
                _wndProcDelegate = HwndHook;
                _oldWndProc = SetWndProc(_wndProcDelegate);
            }

            uint key = 0;
            for(int i = 0; i < keys.Length; i++)
            {
                if (!IsModifierKey(keys[i]))
                    key |= (uint)keys[i];
            }

            HotkeyID = -1;
            bool success = RegisterHotKey(MainWindow.hWnd, HOTKEY_ID, GetModifier(keys), key);
            if (success)
            {
                HotkeyID = HOTKEY_ID;
                hotkeyRegistry.Add((HOTKEY_ID++, hotkeyPressed, obj));
            }
            return success;
        }

        public static bool RegisterHotkey(VirtualKeyModifiers modifier, VirtualKey key, EventHandler hotkeyPressed, out int HotkeyID, object obj = null)
        {
            if (_oldWndProc == IntPtr.Zero)
            {
                _wndProcDelegate = HwndHook;
                _oldWndProc = SetWndProc(_wndProcDelegate);
            }

            HotkeyID = -1;
            bool success = RegisterHotKey(MainWindow.hWnd, HOTKEY_ID, (uint)modifier, (uint)key);
            if (success)
            {
                HotkeyID = HOTKEY_ID;
                hotkeyRegistry.Add((HOTKEY_ID++, hotkeyPressed, obj));
            }
            return success;
        }

        public static void UnregisterHotkey(int HotkeyID)
        {
            UnregisterHotKey(MainWindow.hWnd, HotkeyID);
            for(int i = 0; i < hotkeyRegistry.Count; i++)
            {
                if (hotkeyRegistry[i].HotKeyID == HotkeyID)
                {
                    hotkeyRegistry.RemoveAt(i);
                    return;
                }
            }
        }

        public static void UnregisterAllHotkeys()
        {
            for(int i = 0; i < hotkeyRegistry.Count; i++)
            {
                UnregisterHotKey(MainWindow.hWnd, hotkeyRegistry[i].HotKeyID);
            }
            hotkeyRegistry.Clear();
        }

        public static IntPtr SetWndProc(WndProcDelegate newProc)
        {
            IntPtr functionPointer = Marshal.GetFunctionPointerForDelegate(newProc);
            if (IntPtr.Size == 8)
                return SetWindowLongPtr(MainWindow.hWnd, GWLP_WNDPROC, functionPointer);
            else
                return SetWindowLong(MainWindow.hWnd, GWLP_WNDPROC, functionPointer);
        }

        private static IntPtr HwndHook(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if(msg == WM_HOTKEY)
                for (int i = 0; i < hotkeyRegistry.Count; i++)
                    if (hotkeyRegistry[i].HotKeyID == wParam.ToInt32() && hotkeyRegistry[i].Event != null)
                        hotkeyRegistry[i].Event(null, new ObjectEventArgs() { Obj = hotkeyRegistry[i].obj });
            return CallWindowProc(_oldWndProc, hwnd, msg, wParam, lParam);
        }
    }

    internal class ObjectEventArgs : EventArgs
    {
        public object Obj;
    }
}

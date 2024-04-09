using System;

namespace QuickNav.Helper
{
    internal class WindowHelper
    {
        public static void CenterWindow(IntPtr hWnd, int addWidth = 0, int addHeight = 0)
        {
            Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            if (appWindow is not null)
            {
                Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
                if (displayArea is not null)
                {

                    var width = Math.Clamp(displayArea.WorkArea.Width / 4, 700, 1000) + addWidth;
                    var height = Math.Clamp(displayArea.WorkArea.Height / 4, 500, 700) + addHeight;
                    int x = ((displayArea.WorkArea.Width - width) / 2);
                    int y = ((displayArea.WorkArea.Height - height) / 2);

                    var center = new Windows.Graphics.RectInt32(x, y, width, height);

                    appWindow.MoveAndResize(center);
                }
            }
        }
    }
}

using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.Linq;
using WinUIEx;

namespace QuickNav.Helper
{
    internal class ShowWindowHelper
    {
        public static List<Window> OpenWindows = new();
        public static void CloseAll()
        {
            while (OpenWindows.Count > 0)
            {
                OpenWindows[0].Close();
            }
        }
        public static void ShowWindow<T>(bool singleInstance = true) where T : Window, new()
        {
            if (singleInstance && OpenWindows.Any(x => x.GetType() == typeof(T)))
            {
                var existingWindow = OpenWindows.First(x => x.GetType() == typeof(T));
                existingWindow.Activate();
                return;
            }

            var newWindow = new T();
            newWindow.Activate();
            newWindow.Closed += (sender, args) =>
            {
                OpenWindows.Remove(newWindow);
            };
            OpenWindows.Add(newWindow);
        }
    }
}

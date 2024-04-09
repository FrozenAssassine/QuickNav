using Microsoft.UI.Xaml;
using QuickNav.AppWindows;
using System.Collections.Generic;
using System.Linq;
using WinUIEx;
using static QuickNav.AppWindows.InfoWindow;

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

        public static void ShowInfoWindow(Pages page)
        {
            if (OpenWindows.Any(x => x.GetType() == typeof(InfoWindow)))
            {
                var existingWindow = OpenWindows.First(x => x.GetType() == typeof(InfoWindow));
                existingWindow.Activate();
                (existingWindow as InfoWindow).Select(page);
                return;
            }

            var newWindow = new InfoWindow();
            newWindow.Select(page);
            newWindow.Activate();
            newWindow.Closed += (sender, args) =>
            {
                OpenWindows.Remove(newWindow);
            };
            OpenWindows.Add(newWindow);
        }
    }
}

using H.NotifyIcon;
using QuickNav.Widgets;
using System.Collections.Generic;
using System.Windows.Documents;

namespace QuickNav.Helper
{
    internal class WidgetRegister
    {
        public static List<QuickNavWidget> Widgets = new ();
        public static void StartWidgets()
        {
            RegisterWidget(new DesktopSearchWidget());
        }

        private static void RegisterWidget(QuickNavWidget widget)
        {
            Widgets.Add(widget);
            widget.Activate();
            widget.Show();
        }

        public static void ShowDesktop()
        {
            foreach(var widget in Widgets)
            {
                widget.BringToFront();
            }
        }
    }
}

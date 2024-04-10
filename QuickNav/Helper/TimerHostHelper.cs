using QuickNav.Widgets;
using System.Collections.Generic;

namespace QuickNav.Helper
{
    internal class TimerHostHelper
    {
        public static List<TimerWidget> ActiveTimers = new();
        public static void HostTimer((int hours, int minutes, int seconds) time)
        {
            ActiveTimers.Add(new TimerWidget((time.hours * 3600) + (time.minutes * 60) + time.seconds));
        }
    }
}

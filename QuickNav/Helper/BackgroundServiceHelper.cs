using QuickNavPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace QuickNav.Helper
{
    internal static class BackgroundServiceHelper
    {
        private static Dictionary<IBackgroundService, int> services = new Dictionary<IBackgroundService, int>();

        private static Timer timer;

        public static void Start(IEnumerable<IBackgroundService> s)
        {
            foreach(var service in s)
            {
                services.Add(service, 0);
            }
            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000 * 60; // every minute
            timer.Start();
            Timer_Elapsed(null, null);
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach(var service in services.Keys)
            {
                if (service.GetIntervallMin() <= services[service])
                    service.Elapsed();
                services[service] += 1;
            }
        }
    }
}

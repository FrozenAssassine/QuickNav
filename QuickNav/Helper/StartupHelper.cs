using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Popups;

namespace QuickNav.Helper
{
    public class StartupHelper
    {
        public static async Task<bool> StartupEnabled()
        {
            var startup = await StartupTask.GetAsync("quicknavID1234");
            return GetToggleState(startup.State);
        }

        private static bool GetToggleState(StartupTaskState state)
        {
            return state == StartupTaskState.Enabled;
        }
        public static async Task<bool> ToggleLaunchOnStartup(bool enable)
        {
            var startup = await StartupTask.GetAsync("quicknavID1234");
            switch (startup.State)
            {
                case StartupTaskState.Enabled when !enable:
                    startup.Disable();
                    break;
                case StartupTaskState.Disabled when enable:
                    var updatedState = await startup.RequestEnableAsync();
                    Debug.WriteLine("ENABLED");
                    return GetToggleState(updatedState);
                case StartupTaskState.DisabledByUser when enable:
                    Debug.WriteLine("Unable to change state of startup task via the application - enable via Startup tab on Task Manager (Ctrl+Shift+Esc)");
                    break;
                default:
                    Debug.WriteLine("Unable to change state of startup task");
                    break;
            }
            return false;
        }
    }
}

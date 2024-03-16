using System;
using Microsoft.UI.Windowing;
using System.Threading.Tasks;
using Windows.Devices.Display;
using Windows.Devices.Enumeration;

namespace QuickNav.Helper
{
    internal class WindowHelper
    {
        public static async Task CenterWindow(AppWindow window)
        {

            var displayList = await DeviceInformation.FindAllAsync
                     (DisplayMonitor.GetDeviceSelector());
            
            //TODO: does not work with multiple screens
            var monitorInfo = await DisplayMonitor.FromInterfaceIdAsync(displayList[0].Id);

            var screenHeight = monitorInfo.NativeResolutionInRawPixels.Height;
            var screenWidth = monitorInfo.NativeResolutionInRawPixels.Width;

            var width = Math.Clamp(screenWidth / 4, 400, 800);
            var height = Math.Clamp(screenHeight / 4, 200, 600);

            var windowSize = new Windows.Graphics.RectInt32(screenWidth / 2 - (width / 2), (screenHeight / 2 - (height / 2)), width, height);
            window.MoveAndResize(windowSize);
        }
    }
}

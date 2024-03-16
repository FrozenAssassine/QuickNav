using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.WebSearchCommandCollector
{
    internal static class BrowserInfo // https://stackoverflow.com/a/17599201
    {
        public static string BrowserName = "browser";
        public static Uri BrowserIcon = null;
        
        private static bool isInitialized = false;

        public static void Init()
        {
            if (isInitialized) return;

            const string userChoice = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            string progId;
            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(userChoice))
            {
                if (userChoiceKey == null)
                {
                    return;
                }
                object progIdValue = userChoiceKey.GetValue("Progid");
                if (progIdValue == null)
                {
                    return;
                }
                progId = progIdValue.ToString();
                switch (progId.Split('-')[0])
                {
                    case "IE.HTTP":
                        BrowserName = "Internet Explorer";
                        BrowserIcon = new Uri("ms-appx://App/Assets/browser/ie10.png");
                        break;
                    case "FirefoxURL":
                        BrowserName = "Firefox";
                        BrowserIcon = new Uri("ms-appx://App/Assets/browser/firefox.png");
                        break;
                    case "ChromeHTML":
                        BrowserName = "Chrome";
                        BrowserIcon = new Uri("ms-appx://App/Assets/browser/chrome.png");
                        break;
                    case "OperaStable":
                        BrowserName = "Opera";
                        BrowserIcon = new Uri("ms-appx://App/Assets/browser/opera.png");
                        break;
                    case "SafariHTML":
                        BrowserName = "Safari";
                        BrowserIcon = new Uri("ms-appx://App/Assets/browser/safari.png");
                        break;
                    case "AppXq0fevzme2pys62n3e0fbqa7peapykr8v": // Old Edge version.
                    case "MSEdgeHTM": // Newer Edge version.
                        BrowserName = "Edge";
                        BrowserIcon = new Uri("ms-appx://App/Assets/browser/edge.png");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

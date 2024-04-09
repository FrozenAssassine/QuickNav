using QuickNav.Extensions;
using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Text.Encodings.Web;
using Windows.Services.Store;

namespace QuickNav.BuildInCommands.WebSearchCommandCollector
{
    internal class WebSearchCommand : ICommand
    {
        public string Description => "Run this command to search for your query in web.";

        public Uri Icon => BrowserInfo.BrowserIcon;

        public Priority Priority(string query)
        {
            return QuickNavPlugin.Priority.Low;
        }

        public string CommandTrigger => "net";

        public string[] Keywords => new string[] { "web", "search", "inter", "net" };
        
        public string Name(string query)
        {
            if (query.Length == 0)
                return "Search the web";

            if (query.IsUrl())
                return "Open " + query;

            return "Search for \"" + query + "\" in " + BrowserInfo.BrowserName;
        }

        public bool RunCommand(string parameters, out ContentElement content)
        {
            content = null;

            if (parameters == "")
                return false;

            if (parameters.IsUrl(out Uri uri))
            {
                Windows.System.Launcher.LaunchUriAsync(uri);
                return true;
            }

            Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.google.com/search?q=" + UrlEncoder.Default.Encode(parameters)));
            return true;
        }

        public WebSearchCommand()
        {
            BrowserInfo.Init();
        }

        public void OnWindowOpened() { }
    }
}

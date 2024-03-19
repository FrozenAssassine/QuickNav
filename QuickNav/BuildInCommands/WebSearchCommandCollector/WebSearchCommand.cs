using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Text.Encodings.Web;

namespace QuickNav.BuildInCommands.WebSearchCommandCollector
{
    internal class WebSearchCommand : IUnknownCommandCollector, ITriggerCommand
    {
        public string Description => "Run this command to search for your query in web.";

        public Uri Icon => BrowserInfo.BrowserIcon;

        public Priority Priority => Priority.Low;

        public string CommandTrigger => "net";

        public string[] Keywords => new string[] { "web", "search web", "internet" };

        public string Name(string query)
        {
            if (query.Length == 0)
                return "Search the web";

            return "Search for \"" + query + "\" in " + BrowserInfo.BrowserName;
        }

        public bool RunCommand(string parameters, out ContentElement content)
        {
            content = null;
            Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.google.com/search?q=" + UrlEncoder.Default.Encode(parameters)));
            return true;
        }

        public WebSearchCommand()
        {
            BrowserInfo.Init();
        }
    }
}

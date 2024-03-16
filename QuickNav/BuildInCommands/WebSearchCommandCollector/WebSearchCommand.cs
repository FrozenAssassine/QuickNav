using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.WebSearchCommandCollector
{
    internal class WebSearchCommand : IUnknownCommandCollector
    {
        public string Description => "Run this command to search for your query in web.";

        public Uri Icon => BrowserInfo.BrowserIcon;

        public Priority Priority => Priority.Low;

        public string Name(string query)
        {
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

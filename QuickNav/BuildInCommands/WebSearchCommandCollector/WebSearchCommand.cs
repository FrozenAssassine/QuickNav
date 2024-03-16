using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.WebSearchCommandCollector
{
    internal class WebSearchCommand : IUnknownCommandCollector
    {
        public string Description => "Run this command to search for your query in web.";

        public Uri Icon => null;

        public Priority Priority => Priority.Low;

        public bool RunCommand(string parameters, out ContentElement content) // Extend that with: https://stackoverflow.com/a/17599201 for Icon of the standard browser and standard search engine
        {
            content = null;
            Process.Start("https://www.google.com/search?q=" + UrlEncoder.Default.Encode(parameters));
            return true;
        }
    }
}

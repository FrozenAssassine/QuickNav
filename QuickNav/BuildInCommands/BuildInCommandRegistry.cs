using QuickNav.BuildInCommands.WebSearchCommandCollector;
using QuickNav.Helper;
using QuickNav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands
{
    internal class BuildInCommandRegistry
    {
        public static void Register()
        {
            Plugin webSearch = new Plugin();
            webSearch.Info = new AboutWebSearch();
            webSearch.CollectorCommands.Add(new WebSearchCommand());
            PluginHelper.Plugins.Add(webSearch);
        }
    }
}

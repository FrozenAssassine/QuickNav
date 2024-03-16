﻿using QuickNav.BuildInCommands.FileSearchCommandCollector;
using QuickNav.BuildInCommands.WebSearchCommandCollector;
using QuickNav.BuildInCommands.WindowsFileSearch;
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

            Plugin fileSearch = new Plugin();
            fileSearch.Info = new AboutFileSearch();
            fileSearch.CollectorCommands.Add(new FileSearchCommand());


            PluginHelper.Plugins.Add(webSearch);
            PluginHelper.Plugins.Add(fileSearch);
        }
    }
}
using QuickNavPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.WebSearchCommandCollector
{
    internal class AboutWebSearch : IAboutInfo
    {
        public string Name => "Web Search";

        public string Description => "This plugin provides you a search command if no other actions are available.";

        public string Author => "Finn Freitag";

        public string AuthorUrl => "https://github.com/finn-freitag";

        public Uri Icon => null;

        public string Url => "https://github.com/FrozenAssassine/QuickNav";
    }
}

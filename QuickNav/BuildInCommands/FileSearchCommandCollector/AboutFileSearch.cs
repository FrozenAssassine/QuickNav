using QuickNavPlugin;
using System;

namespace QuickNav.BuildInCommands.FileSearchCommandCollector
{
    internal class AboutFileSearch : IAboutInfo
    {
        public string Name => "Windows File Search";

        public string Description => "This plugin searches the windows system for files";

        public string Author => "Julius Kirsch";

        public string AuthorUrl => "https://github.com/FrozenAssassine";

        public Uri Icon => null;

        public string Url => "https://github.com/FrozenAssassine/QuickNav";
    }
}
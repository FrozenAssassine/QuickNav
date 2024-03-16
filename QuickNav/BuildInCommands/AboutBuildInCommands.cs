using QuickNavPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands
{
    internal class AboutBuildInCommands : IAboutInfo
    {
        public string Name => "Build in commands";

        public string Description => "Some general functionality like browser and file search, command line, etc.";

        public string Author => "Julius Kirsch, Finn Freitag";

        public string AuthorUrl => "https://github.com/FrozenAssassine/QuickNav";

        public Uri Icon => new Uri("ms-appx://App/Assets/StoreLogo.png");

        public string Url => "https://github.com/FrozenAssassine/QuickNav";
    }
}

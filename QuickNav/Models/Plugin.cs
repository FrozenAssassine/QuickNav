using QuickNavPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.Models
{
    internal class Plugin
    {
        public List<ICommand> Commands = new List<ICommand>();
        public List<IBackgroundService> Services = new List<IBackgroundService>();
        public IAboutInfo Info = null;
    }
}

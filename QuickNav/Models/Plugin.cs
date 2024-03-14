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
        public List<ITriggerCommand> TriggerCommands = new List<ITriggerCommand>();
        public List<IUnknownCommandCollector> CollectorCommands = new List<IUnknownCommandCollector>();
        public IAboutInfo Info = null;
    }
}

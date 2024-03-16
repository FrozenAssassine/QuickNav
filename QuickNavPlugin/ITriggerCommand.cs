using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin
{
    public interface ITriggerCommand : ICommand
    {
        /// <summary>
        /// Command Trigger, for example if you have a command-line plugin use ">" as trigger. With that the application can find your command. Leave it empty to let the user just find your command via keywords.
        /// </summary>
        string CommandTrigger { get; }
        /// <summary>
        /// Keywords with which your command can be found.
        /// </summary>
        string[] Keywords { get; }
    }
}

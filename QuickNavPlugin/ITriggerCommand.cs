using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin
{
    public interface ITriggerCommand : ICommand
    {
        string CommandTrigger { get; }
        string[] Keywords { get; }
    }
}

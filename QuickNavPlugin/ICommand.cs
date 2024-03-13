using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickNavPlugin.UI;

namespace QuickNavPlugin
{
    public interface ICommand : IPlugin
    {
        string Description { get; }
        Uri Icon { get; }
        Priority Priority { get; }
        bool RunCommand(string parameters, out ContentElement content);
    }
}

using Calculator;
using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.CalculatorCommandCollector
{
    internal class CalculatorCommand : IUnknownCommandCollector
    {
        public string Description => "Calculate terms.";

        public Uri Icon => null;

        public QuickNavPlugin.Priority Priority => QuickNavPlugin.Priority.High;

        public string Name(string query)
        {
            return "Calculate \"" + query + "\"";
        }

        public bool RunCommand(string parameters, out ContentElement content)
        {
            try
            {
                content = new LabelElement(Parser.Parse(parameters).Calc().ToString());
                return true;
            }
            catch
            {
                content = null;
                return false;
            }
        }
    }
}

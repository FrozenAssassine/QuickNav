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
    internal class CalculatorCommand : IUnknownCommandCollector, ITriggerCommand
    {
        public string Description => "Calculate terms.";

        public Uri Icon => new Uri("ms-appx://App/Assets/commands/calculator.png");

        public QuickNavPlugin.Priority Priority => QuickNavPlugin.Priority.Low;

        public string CommandTrigger => "=";

        public string[] Keywords => new string[] { "calc", "calculator" };

        public string Name(string query)
        {
            if (query == "") return "Calculate";
            try
            {
                return "= " + Parser.Parse(query.StartsWith(CommandTrigger) ? query.Substring(CommandTrigger.Length).TrimStart() : query).Calc().ToString().Replace(',', '.');
            }
            catch
            {
                return "Calculate \"" + query + "\"";
            }
        }

        public bool RunCommand(string parameters, out ContentElement content)
        {
            if(parameters == "")
            {
                content = null;
                return false;
            }

            try
            {
                content = new LabelElement(Parser.Parse(parameters).Calc().ToString().Replace(',', '.'));
                return true;
            }
            catch (Exception e)
            {
                content = new LabelElement("Parsing error!");
                return true;
            }
        }
    }
}

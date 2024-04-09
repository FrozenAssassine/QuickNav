using Calculator;
using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.CalculatorCommandCollector
{
    internal class CalculatorCommand : ICommand
    {
        public string Description => "Calculate terms.";

        public Uri Icon => new Uri("ms-appx://App/Assets/commands/calculator.png");

        public QuickNavPlugin.Priority Priority(string query)
        {
            if (query.Length == 0 || !IsMathOperation(query))
                return QuickNavPlugin.Priority.Invisible;
            try
            {
                Parser.Parse(query).Calc();
                return QuickNavPlugin.Priority.High;
            }
            catch
            {
                return QuickNavPlugin.Priority.Invisible;
            }
        }

        public string CommandTrigger => "=";

        public string[] Keywords => new string[] { "calc", "calculator" };


        public bool IsMathOperation(string query)
        {
            //Not sure? product -> class FProduct : IFunction; 
            string[] allowedOps = new string[] { "(", ")", "+", "-", "*", "/", "%", "^", "π", "\\pi", "\\e", "cos", "sin", "tan", "asin", "acos", "sqrt", "log", "abs", "sum", "product", "⌈", "⌉", "⌊", "⌋", "atan", "[", "]" };

            for(int i = 0; i<query.Length; i++)
            {
                if (char.IsNumber(query[i]) || query[i] == 'π' || (query[i] == '\\' && query.Length > i + 1 && query[i + 1] == 'e') || (query[i] == '\\' && query.Length > i + 2 && query[i + 1] == 'p' && query[i + 2] == 'i'))
                    return true;
            }

            for (int i = 0; i < allowedOps.Length; i++)
            {
                if (query.Contains(allowedOps[i], StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public string Name(string query)
        {
            bool isMath = IsMathOperation(query);
            Debug.WriteLine(isMath);
            if (query.Length == 0 || !isMath)
                return "Calculate";
            try
            {
                return "= " + ClearNegativeZero(((double)decimal.Round(Parser.Parse(query).Calc(), 15)).ToString().Replace(',', '.'));
            }
            catch
            {
                return "Calculate \"" + query + "\"";
            }
        }

        public bool RunCommand(string parameters, out ContentElement content)
        {
            if(parameters.Length == 0)
            {
                content = null;
                return false;
            }

            try
            {
                content = new LabelElement(ClearNegativeZero(((double)decimal.Round(Parser.Parse(parameters).Calc(), 15)).ToString().Replace(',', '.')));
                return true;
            }
            catch (Exception e)
            {
                content = new LabelElement("Parsing error!");
                return true;
            }
        }

        public CalculatorCommand()
        {
            CommandSettings.MaxTaylorIterations = 100;
        }

        private string ClearNegativeZero(string val)
        {
            if (val == "-0")
                return "0";
            return val;
        }

        public void OnWindowOpened() { }
    }
}

using Calculator;
using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;


namespace QuickNav.BuildInCommands.CalculatorCommandCollector
{
    internal class CalculatorCommand : ICommand
    {
        public string Description => "Calculate terms.";

        public Uri Icon(string query) => new Uri("ms-appx://App/Assets/commands/calculator.png");

        public QuickNavPlugin.Priority Priority(string query)
        {
            if (query == "" || IsWrongAlert(query))
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

        public string Name(string query)
        {
            if (query == "")
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
            if(parameters == "" || IsWrongAlert(parameters))
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

        private bool IsWrongAlert(string query)
        {
            return (query.Length == 2
                && char.IsLetter(query[0])
                && char.IsLetter(query[1]))
                || (query.Length == 1
                && char.IsLetter(query[0]));
        }

        public void OnWindowOpened() { }
    }
}

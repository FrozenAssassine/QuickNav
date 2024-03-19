using QuickNav.BuildInCommands.CalculatorCommandCollector;
using QuickNav.BuildInCommands.CMDCommandCollector;
using QuickNav.BuildInCommands.FileInfoCommandCollector;
using QuickNav.BuildInCommands.PlainTextConverterCommandCollector;
using QuickNav.BuildInCommands.WebSearchCommandCollector;
using QuickNav.BuildInCommands.WindowsFileSearch;
using QuickNav.Helper;
using QuickNav.Models;

namespace QuickNav.BuildInCommands
{
    internal class BuildInCommandRegistry
    {
        public static void Register()
        {
            Plugin buildInCommands = new Plugin();
            buildInCommands.Info = new AboutBuildInCommands();
            buildInCommands.CollectorCommands.Add(new WebSearchCommand());
            buildInCommands.CollectorCommands.Add(new FileSearchCommand());
            buildInCommands.CollectorCommands.Add(new CalculatorCommand());
            buildInCommands.CollectorCommands.Add(new CMDCommand());
            buildInCommands.TriggerCommands.Add(new CountWordsCommand());
            buildInCommands.TriggerCommands.Add(new CMDCommand());
            buildInCommands.TriggerCommands.Add(new CountWordsCommand());
            buildInCommands.TriggerCommands.Add(new FileInfoCommand());
            buildInCommands.TriggerCommands.Add(new PlainTextConverterCommand());
            buildInCommands.TriggerCommands.Add(new CalculatorCommand());

            PluginHelper.Plugins.Add(buildInCommands);
        }
    }
}

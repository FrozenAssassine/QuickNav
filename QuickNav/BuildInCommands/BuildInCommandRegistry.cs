using QuickNav.BuildInCommands.CalculatorCommandCollector;
using QuickNav.BuildInCommands.CMDCommandCollector;
using QuickNav.BuildInCommands.ColorPickerCommandTrigger;
using QuickNav.BuildInCommands.FileInfoCommandCollector;
using QuickNav.BuildInCommands.LockScreenCommandCollector;
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
            // Don't create two instances of the same command:
            var webcmd = new WebSearchCommand();
            var filecmd = new FileSearchCommand();
            var calccmd = new CalculatorCommand();
            var cmdcmd = new CMDCommand();
            var wordcmd = new CountWordsCommand();

            Plugin buildInCommands = new Plugin();
            buildInCommands.Info = new AboutBuildInCommands();
            buildInCommands.CollectorCommands.Add(webcmd);
            buildInCommands.CollectorCommands.Add(filecmd);
            buildInCommands.CollectorCommands.Add(calccmd);
            buildInCommands.CollectorCommands.Add(cmdcmd);
            buildInCommands.CollectorCommands.Add(wordcmd);
            buildInCommands.TriggerCommands.Add(webcmd);
            buildInCommands.TriggerCommands.Add(filecmd);
            buildInCommands.TriggerCommands.Add(cmdcmd);
            buildInCommands.TriggerCommands.Add(wordcmd);
            buildInCommands.TriggerCommands.Add(new FileInfoCommand());
            buildInCommands.TriggerCommands.Add(new PlainTextConverterCommand());
            buildInCommands.TriggerCommands.Add(calccmd);
            buildInCommands.TriggerCommands.Add(new ColorPickerCommand());
            buildInCommands.TriggerCommands.Add(new LockScreenCommand());

            PluginHelper.Plugins.Add(buildInCommands);
        }
    }
}

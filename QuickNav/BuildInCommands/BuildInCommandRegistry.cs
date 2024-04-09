using QuickNav.BuildInCommands.CalculatorCommandCollector;
using QuickNav.BuildInCommands.CMDCommandCollector;
using QuickNav.BuildInCommands.ColorPickerCommandTrigger;
using QuickNav.BuildInCommands.FileInfoCommandCollector;
using QuickNav.BuildInCommands.LaunchAppCommandCollector;
using QuickNav.BuildInCommands.LockScreenCommandCollector;
using QuickNav.BuildInCommands.PlainTextConverterCommandCollector;
using QuickNav.BuildInCommands.SystemMonitorCommandCollector;
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
            buildInCommands.Commands.Add(new WebSearchCommand());
            buildInCommands.Commands.Add(new FileSearchCommand());
            buildInCommands.Commands.Add(new CalculatorCommand());
            buildInCommands.Commands.Add(new CMDCommand());
            buildInCommands.Commands.Add(new CountWordsCommand());
            buildInCommands.Commands.Add(new FileInfoCommand());
            buildInCommands.Commands.Add(new PlainTextConverterCommand());
            buildInCommands.Commands.Add(new ColorPickerCommand());
            buildInCommands.Commands.Add(new LockScreenCommand());
            buildInCommands.Commands.Add(new SysInfoCommand());
            buildInCommands.Commands.Add(new LaunchAppCommand());

            PluginHelper.Plugins.Add(buildInCommands);
        }
    }
}

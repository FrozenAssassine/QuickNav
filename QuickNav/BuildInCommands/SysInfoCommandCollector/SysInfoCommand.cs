using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Management;


namespace QuickNav.BuildInCommands.SystemMonitorCommandCollector;

internal class SysInfoCommand : ITriggerCommand
{
    public string Description => "Monitor your system Stats";

    public Uri Icon => new Uri("ms-appx://App/Assets/commands/clipboardtext.png");

    public Priority Priority => Priority.Low;

    public string CommandTrigger => "perf"; //performance

    public string[] Keywords => new string[] { "performance", "cpu", "gpu", "ram" };

    public string Name(string query)
    {
        return "Run to get infos about CPU, RAM and GPU";
    }

    public bool RunCommand(string file, out ContentElement content)
    {
        var grid = new GridElement(2, Orientation.Vertical);
        content = grid;

        grid.Children.Add();

        return true;
    }
}
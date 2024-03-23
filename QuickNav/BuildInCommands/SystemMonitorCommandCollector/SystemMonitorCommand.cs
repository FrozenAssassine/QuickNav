using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Diagnostics;
using System.Timers;

namespace QuickNav.BuildInCommands.SystemMonitorCommandCollector;

internal class SystemMonitorCommand : ITriggerCommand
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
        var cpuLabel = new LabelElement("");
        var ramLabel = new LabelElement("");

        Timer t = new Timer();
        t.Interval = 2000;
        t.Start();
        t.Elapsed += delegate
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            float cpuUsage = cpuCounter.NextValue();

            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            double ramUsage = Math.Round(ramCounter.RawValue / 1024.0, 2);

            cpuLabel.Text = "CPU: " + cpuUsage + "%";
            ramLabel.Text = "RAM: " + ramUsage + "GB";
        };

        var grid = new GridElement(2, Orientation.Horizontal);
        content = grid;

        grid.Children.Add(cpuLabel);
        grid.Children.Add(ramLabel);

        return true;
    }
}
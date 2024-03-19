using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.CMDCommandCollector;

internal class CMDCommand : ITriggerCommand, IUnknownCommandCollector
{
    public string Description => "Run this command to execute in commandline";

    public Uri Icon => new Uri("ms-appx://App/Assets/commands/cmd.png");

    public Priority Priority => Priority.Low;

    public string CommandTrigger => ">";

    public string[] Keywords => new string[] { "cmd", "command", "run" };

    public string Name(string query)
    {
        if (query == "") return "Run";
        return "Run \"" + query + "\"";
    }

    public bool RunCommand(string command, out ContentElement output)
    {
        LabelElement outputLabel = new LabelElement();
        output = outputLabel;
        outputLabel.Scrollable = outputLabel.AutoScrollBottom = true;

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = $"/C {command}"
        };

        // Assign start info to the process
        process.StartInfo = startInfo;

        // Set up event handlers for output and error
        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                MainWindow.dispatcherQueue.TryEnqueue(() => outputLabel.Text += e.Data + Environment.NewLine);
            }
        };
        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                MainWindow.dispatcherQueue.TryEnqueue(() => outputLabel.Text += e.Data + Environment.NewLine);
            }
        };
        process.Start();

        Task.Run(() =>
        {
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
            process.Close();
        });

        return true;
    }
}
using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Diagnostics;

namespace QuickNav.BuildInCommands.CMDCommandCollector;

internal class CMDCommand : IUnknownCommandCollector
{
    public string Description => "Run this command to execute in commandline";

    public Uri Icon => null;

    public Priority Priority => Priority.Low;

    public string Name(string query)
    {
        return "Run \"" + query + "\"";
    }

    private LabelElement outputLabel = new LabelElement();
    

    public bool RunCommand(string command, out ContentElement content)
    {
        content = outputLabel;

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
        process.OutputDataReceived += (sender, e) => outputLabel.Text += e.Data;
        process.ErrorDataReceived += (sender, e) => outputLabel.Text += e.Data;

        // Start the process
        process.Start();

        string output = process.StandardOutput.ReadLine();

        // Wait for the process to exit
        process.WaitForExit();

        // Close the process
        process.Close();

        Debug.WriteLine("Command executed successfully.");

        return true;
    }
}
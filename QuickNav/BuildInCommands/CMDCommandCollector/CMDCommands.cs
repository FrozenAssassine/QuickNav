﻿using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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

    public bool RunCommand(string command, out ContentElement output)
    {
        LabelElement outputLabel = new LabelElement();
        output = outputLabel;
        outputLabel.FontSize = 16;
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
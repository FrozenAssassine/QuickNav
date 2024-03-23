﻿using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Text;
using QuickNav.Extensions;
using System.IO;

namespace QuickNav.BuildInCommands.SystemMonitorCommandCollector;

internal class SysInfoCommand : ITriggerCommand
{
    public string Description => "See your system informations";

    public Uri Icon => null; //Todo for Finn

    public string CommandTrigger => "sysinfo"; //system informations

    public string[] Keywords => new string[] { "system", "informations", "sysinf" };

    public string Name(string query)
    {
        return "Get informations about your system";
    }

    public bool RunCommand(string file, out ContentElement content)
    {
        StringBuilder markdown = new StringBuilder();
        markdown.AppendLine("## Device Information");
        markdown.AppendMarkdownLine("**Device Name:** " + Environment.MachineName);
        markdown.AppendMarkdownLine("**Username:** " + Environment.UserName);

        markdown.AppendLine("## CPU Information");
        markdown.AppendMarkdownLine("**Name:** " + GetCPUName());
        markdown.AppendMarkdownLine("**Threads:** " + Environment.ProcessorCount);

        markdown.AppendLine("## Windows Version");
        markdown.AppendMarkdownLine("**Version:** " + Environment.OSVersion.Version);
        markdown.AppendMarkdownLine("**Service Pack:** " + Environment.OSVersion.ServicePack);

        markdown.AppendLine("## Disk Information");
        var drives = Environment.GetLogicalDrives();
        foreach (var drive in drives)
        {
            markdown.AppendMarkdownLine("**Drive:** " + drive);
            markdown.AppendMarkdownLine("**Total Size:** " + GetDriveTotalSize(drive) + " GB");
            markdown.AppendMarkdownLine("**Free Space:** " + GetDriveFreeSpace(drive) + " GB");
            markdown.AppendMarkdownLine("");
        }

        var mdRenderer = new MarkdownElement(markdown.ToString());
        content = mdRenderer;
        return true;
    }

    private string GetCPUName()
    {
        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0"))
        {
            if (key != null)
            {
                return key.GetValue("ProcessorNameString").ToString();
            }
        }
        return "Unknown";
    }
    private double GetDriveTotalSize(string drive)
    {
        try
        {
            DriveInfo driveInfo = new DriveInfo(drive);
            return Math.Round(driveInfo.TotalSize / (1024.0 * 1024.0 * 1024.0), 2);
        }
        catch (Exception)
        {
            return -1;
        }
    }
    private double GetDriveFreeSpace(string drive)
    {
        try
        {
            DriveInfo driveInfo = new DriveInfo(drive);
            return Math.Round(driveInfo.AvailableFreeSpace / (1024.0 * 1024.0 * 1024.0), 2);
        }
        catch (Exception)
        {
            return -1;
        }
    }

    Priority ICommand.Priority(string query)
    {
        return QuickNavPlugin.Priority.Low;
    }
}
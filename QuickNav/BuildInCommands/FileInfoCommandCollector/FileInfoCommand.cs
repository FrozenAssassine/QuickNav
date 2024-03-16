using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.IO;

namespace QuickNav.BuildInCommands.FileInfoCommandCollector;

internal class FileInfoCommand : ITriggerCommand
{
    public string Description => "Run this command to get infos about a file";

    public Uri Icon => null;

    public Priority Priority => Priority.Low;

    public string CommandTrigger => "finf";

    public string[] Keywords => new string[] { "info", "fileinfo"};

    public string Name(string query)
    {
        return "Search for \"" + query + "\" in your files";
    }

    public bool RunCommand(string file, out ContentElement content)
    {
        var textLabel = new LabelElement();
        content = textLabel;

        FileInfo fileInfo = new FileInfo(file);

        string fileInfoString = $"File Name: {fileInfo.Name}\n" +
                                $"File Path: {fileInfo.FullName}\n" +
                                $"File Size: {fileInfo.Length} bytes\n" +
                                $"File Extension: {fileInfo.Extension}\n" +
                                $"Date Modified: {fileInfo.LastWriteTime}\n";

        textLabel.Text = fileInfoString;

        return true;
    }
}
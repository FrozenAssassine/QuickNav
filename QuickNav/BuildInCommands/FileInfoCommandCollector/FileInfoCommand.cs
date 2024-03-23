using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.IO;

namespace QuickNav.BuildInCommands.FileInfoCommandCollector;

internal class FileInfoCommand : ICommand, IFileCommand
{
    public string Description => "Get infos about a file";

    public Uri Icon => new Uri("ms-appx://App/Assets/commands/fileinfo.png");

    public Priority Priority(string query)
    {
        if (query == "")
            return QuickNavPlugin.Priority.Invisible;
        if (File.Exists(query.Trim().Trim('\"')))
            return QuickNavPlugin.Priority.Medium;
        return QuickNavPlugin.Priority.Invisible;
    }

    public string CommandTrigger => "fileinfo:";

    public string[] Keywords => new string[] { "info", "file"};

    public string[] ExtensionFilter => new string[0];

    public string Name(string query)
    {
        if (query.Length == 0)
            return "Informations about a file";
        return "Informations about \"" + query.Trim().Trim('\"') + "\"";
    }

    public bool RunCommand(string file, out ContentElement content)
    {
        var textLabel = new LabelElement();
        content = textLabel;

        file = file.Trim().Trim('\"');

        if (!File.Exists(file))
        {
            //TODO show error that file does not exist
            return false;
        }
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
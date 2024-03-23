﻿using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.IO;
using Windows.Storage.FileProperties;
using Windows.Storage;
using System.Text;
using QuickNav.Extensions;
using MetadataExtractor;
using System.Collections.Generic;
using System.Net.Mime;
using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.Formats.Avi;
using MetadataExtractor.Formats.Mpeg;
using System.Diagnostics;

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

    static bool IsImage(string filePath)
    {
        string extension = Path.GetExtension(filePath);
        return extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
               extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) ||
               extension.Equals(".png", StringComparison.OrdinalIgnoreCase) ||
               extension.Equals(".gif", StringComparison.OrdinalIgnoreCase) ||
               extension.Equals(".bmp", StringComparison.OrdinalIgnoreCase);
    }
    static bool IsVideo(string filePath)
    {
        string extension = Path.GetExtension(filePath);
        return extension.Equals(".mp4", StringComparison.OrdinalIgnoreCase) ||
               extension.Equals(".avi", StringComparison.OrdinalIgnoreCase) ||
               extension.Equals(".mov", StringComparison.OrdinalIgnoreCase) ||
               extension.Equals(".mkv", StringComparison.OrdinalIgnoreCase);
    }
    static bool IsAudio(string filePath)
    {
        string extension = Path.GetExtension(filePath);
        return extension.Equals(".mp3", StringComparison.OrdinalIgnoreCase) ||
               extension.Equals(".wav", StringComparison.OrdinalIgnoreCase) ||
               extension.Equals(".ogg", StringComparison.OrdinalIgnoreCase) ||
               extension.Equals(".flac", StringComparison.OrdinalIgnoreCase);
    }


    public bool RunCommand(string file, out ContentElement content)
    {
        content = null;

        file = file.Trim().Trim('\"');

        if (!File.Exists(file))
        {
            //TODO show error that file does not exist
            return false;
        }

        FileInfo fileInfo = new FileInfo(file);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"#Info {fileInfo.Name}");
        sb.AppendMarkdownLine($"**Path:** {file}");
        sb.AppendMarkdownLine($"**Size:** {fileInfo.Length}");
        sb.AppendMarkdownLine($"**Extension:** {fileInfo.Extension}");
        sb.AppendMarkdownLine($"**Date Modified:** {fileInfo.LastWriteTime}");

        try
        {

            IEnumerable<MetadataExtractor.Directory> directories = null;
            if (IsAudio(file))
            {
                directories = Mp3MetadataReader.ReadMetadata(File.OpenRead(file));
            }
            else if (IsVideo(file))
            {
                directories = AviMetadataReader.ReadMetadata(file);
            }
            else if(IsImage(file))
            {
                directories = ImageMetadataReader.ReadMetadata(file);
            }

            if (directories == null)
                return false;

            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                {
                    sb.AppendMarkdownLine($"**{tag.Name}:** {tag.Description}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
        }

        content = new MarkdownElement(sb.ToString());

        return true;
    }
}
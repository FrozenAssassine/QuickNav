using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace QuickNav.BuildInCommands.WindowsFileSearch;

internal class FileSearchCommand : IUnknownCommandCollector
{
    public string Description => "Run this command to search files on windows";

    public Uri Icon => null;

    public Priority Priority => Priority.Low;

    public bool RunCommand(string parameters, out ContentElement content)
    {
        content = null;

        try
        {
            dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));
            var searchFolder = shell.Namespace("search-ms:");

            var searchResults = searchFolder.Items().Item(parameters);

            //TODO implement into UI:
            foreach (var item in searchResults)
            {
                Debug.WriteLine(item.Path);
            }
        }
        catch (COMException ex)
        {
            Debug.WriteLine("An error occurred: " + ex.Message);
        }

        return true;
    }
}
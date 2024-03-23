using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Diagnostics;
using System.Data.OleDb;
using QuickNav.Helper;
using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace QuickNav.BuildInCommands.LaunchAppCommandCollector;

internal class LaunchAppCommand : IUnknownCommandCollector, ITriggerCommand
{
    public string Description => "Run this command to launch an app";

    public Uri Icon => null; //Todo for Finn

    public Priority Priority => Priority.Low;

    public string CommandTrigger => "app:";

    public string[] Keywords => new string[] { "app", "launch"};

    public string Name(string query)
    {
        if (query == "") return "Launch an app";
        return "Launch \"" + query + "\"";
    }

    private void SearchInstalledApps(string keyPath, string searchTerm, ListViewElement listViewElement)
    {
        using (var uninstallKey = Registry.LocalMachine.OpenSubKey(keyPath))
        {
            if (uninstallKey == null)
                return;

            List<LabelElement> displayNames = uninstallKey
                .GetSubKeyNames()
                .Select(subKeyName => uninstallKey.OpenSubKey(subKeyName))
                .Where(appKey => appKey != null)
                .Select(appKey => appKey.GetValue("DisplayName"))
                .Where(displayName => displayName != null && displayName.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Select(displayName => new LabelElement(displayName.ToString()))
                .ToList();

            foreach (var item in displayNames)
                listViewElement.Children.Add(item);
        }
    }

    public bool RunCommand(string searchTerm, out QuickNavPlugin.UI.ContentElement content)
    {
        var listViewElement = new ListViewElement();
        content = listViewElement;
        listViewElement.Orientation = Orientation.Vertical;
        listViewElement.Children.Clear();

        //win32 apps:
        SearchInstalledApps(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", searchTerm, listViewElement);

        //store apps:
        SearchInstalledApps(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Appx\AppxAllUserStore", searchTerm, listViewElement);

        return true;
    }
}
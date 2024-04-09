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
using Microsoft.WindowsAPICodePack.Shell;
using QuickNav.Views;
using QuickNav.Models;
using Microsoft.UI.Xaml.Controls;

namespace QuickNav.BuildInCommands.LaunchAppCommandCollector;

internal class LaunchAppCommand : ICommand, IBuildInCommand
{
    public string Description => "Run this command to launch an app";

    public Uri Icon => null; //Todo for Finn

    public string CommandTrigger => "app:";

    public string[] Keywords => new string[] { "app", "launch" };

    public string Name(string query)
    {
        if (query == "") return "Launch an app";
        return "Launch \"" + query + "\"";
    }

    Priority ICommand.Priority(string query)
    {
        return Priority.Medium;
    }

    public bool RunCommand(string searchTerm, out Page content, out double addWidth, out double addHeight)
    {
        content = null;
        addWidth = 0;
        addHeight = 0;

        var appView = new SearchedAppsView();

        // GUID taken from https://learn.microsoft.com/en-us/windows/win32/shell/knownfolderid
        var FOLDERID_AppsFolder = new Guid("{1e87508d-89c2-42f0-8a7e-645a0f50ca58}");
        ShellObject appsFolder = (ShellObject)KnownFolderHelper.FromKnownFolderId(FOLDERID_AppsFolder);

        var apps = ((IKnownFolder)appsFolder).Where(x => x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

        if (apps.Count() == 1)
            appView.LaunchApp(apps.ElementAt(0).ParsingName);
        else
        {
            appView.ShowApps(apps);
            content = appView;
        }
        
        return true;
    }

    public bool RunCommand(string parameters, out QuickNavPlugin.UI.ContentElement content)
    {
        throw new NotImplementedException();
    }
}
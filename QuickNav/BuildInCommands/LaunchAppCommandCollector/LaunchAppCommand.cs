﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.WindowsAPICodePack.Shell;
using QuickNav.Models;
using QuickNav.Views;
using QuickNavPlugin;
using System;
using System.Linq;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace QuickNav.BuildInCommands.LaunchAppCommandCollector;

internal class LaunchAppCommand : ICommand, IBuildInCommand
{
    public string Description => "Run this command to launch an app";

    public string CommandTrigger => "app:";

    public string[] Keywords => new string[] { "app", "launch" };

    private string oldQuery = "";
    private string OldName = "";
    private BitmapImage OldIcon = null;
    public string Name(string query)
    {
        FoundApp = false;
        if (query.Length == 0)
            return OldName = "Launch an app";

        if (oldQuery.Equals(query, StringComparison.Ordinal))
            return OldName;
        
        var apps = Apps.Where(x => x.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToArray();
        if (FoundApp = apps.Length == 1)
            return OldName = "Launch \"" + apps[0].Name + "\"";

        return OldName = "Search \"" + query + "\" in your Apps";
    }

    bool FoundApp = false;

    Priority ICommand.Priority(string query)
    {
        if(FoundApp)
            return Priority.High;

        return Priority.Medium;
    }
    ShellObject[] Apps;

    public void OnWindowOpened()
    {
        oldQuery = "";
        FoundApp = false;

        // GUID taken from https://learn.microsoft.com/en-us/windows/win32/shell/knownfolderid
        var FOLDERID_AppsFolder = new Guid("{1e87508d-89c2-42f0-8a7e-645a0f50ca58}");
        ShellObject appsFolder = (ShellObject)KnownFolderHelper.FromKnownFolderId(FOLDERID_AppsFolder);

        Apps = ((IKnownFolder)appsFolder).ToArray();
    }

    public bool RunCommand(string searchTerm, out Page content, out double addWidth, out double addHeight)
    {
        content = null;
        addWidth = 0;
        addHeight = 0;

        var appView = new SearchedAppsView();

        var apps = Apps.Where(x => x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

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
        content = null;
        return false;
    }

    public ImageSource Icon(string query)
    {
        FoundApp = false;
        if (query.Length == 0 || oldQuery.Equals(query, StringComparison.Ordinal))
            return OldIcon = new BitmapImage(new Uri("ms-appx://App/Assets/commands/launch.png"));

        var apps = Apps.Where(x => x.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToArray();
        if (FoundApp = apps.Length == 1)
        {
            //TODO returnn icon for the current app
            //return ConvertHelper.GetWinUI3BitmapSourceFromIcon(apps[0].Thumbnail.LargeIcon);
        }

        return OldIcon = new BitmapImage(new Uri("ms-appx://App/Assets/commands/launch.png"));
    }

    public ImageSource IconAsync(string query)
    {
        FoundApp = false;
        if (query.Length == 0 || oldQuery.Equals(query, StringComparison.Ordinal))
            return OldIcon = new BitmapImage(new Uri("ms-appx://App/Assets/commands/launch.png"));

        var apps = Apps.Where(x => x.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToArray();
        if (FoundApp = apps.Length == 1)
        {
            //return apps[0].Thumbnail.LargeIcon;
        }


        return new BitmapImage(new Uri("ms-appx://App/Assets/commands/launch.png"));
    }
}
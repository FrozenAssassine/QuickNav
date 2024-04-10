using Newtonsoft.Json;
using QuickNav.Core;
using QuickNav.Models;
using QuickNavPlugin;
using System.Collections.Generic;

namespace QuickNav.Helper;

internal static class CommandAutostartHelper
{
    public static List<AutostartConfigurationItem> AutostartCommands = new List<AutostartConfigurationItem>();
    public delegate void RunCommandCallback(string query, ICommand cmd);
    public static RunCommandCallback Callback;

    public static AutostartConfigurationItem GetItemFromCommand(ICommand command)
    {
        if (AutostartCommands == null)
            return null;

        var uid = PluginHelper.GetUniqueCommandID(command);
        int index = AutostartCommands.FindIndex(x => x.UniqueCommandID == uid);
        if (index == -1)
            return null;
        return AutostartCommands[index];

    }

    public static void RunCommands()
    {
        if (Callback == null)
            return;

        for(int i = 0; i < AutostartCommands.Count; i++)
        {
            Callback(AutostartCommands[i].Query, PluginHelper.GetCommandFromUniqueID(AutostartCommands[i].UniqueCommandID));
        }
    }

    public static void AddCommand(ICommand cmd, string query)
    {
        string uid = PluginHelper.GetUniqueCommandID(cmd);
        int index = AutostartCommands.FindIndex(x => x.UniqueCommandID == uid);
        if (index == -1)
            AutostartCommands.Add(new AutostartConfigurationItem { Query = query, UniqueCommandID = uid });

        SaveAll();
    }

    public static void RemoveCommand(ICommand cmd)
    {
        string uid = PluginHelper.GetUniqueCommandID(cmd);
        int index = AutostartCommands.FindIndex(x => x.UniqueCommandID == uid);
        if (index > -1)
            AutostartCommands.RemoveAt(index);

        SaveAll();
    }

    public static void SaveAll()
    {
        string data = JsonConvert.SerializeObject(AutostartCommands);
        AppSettings.SaveSettings(AppSettingsValues.PluginAutostart, data);
    }

    public static void LoadAll()
    {
        var pluginCommands = AppSettings.GetSettings(AppSettingsValues.PluginAutostart);
        AutostartCommands = JsonConvert.DeserializeObject<List<AutostartConfigurationItem>>(pluginCommands);

        if (AutostartCommands == null)
            AutostartCommands = new();
    }
}

using Newtonsoft.Json;
using QuickNav.Core;
using QuickNav.Models;
using QuickNavPlugin;
using System;
using System.Collections.Generic;
using Windows.System;

namespace QuickNav.Helper;

internal static class CommandShortcutHelper
{
    private static List<ShortcutConfigurationItem> Shortcuts;
    public delegate void RunCommandCallback(string query, ICommand cmd);
    public static RunCommandCallback Callback;

    public static void GetShortcuts()
    {
        var pluginCommands = AppSettings.GetSettings(AppSettingsValues.PluginShortcuts);
        Shortcuts = JsonConvert.DeserializeObject< List<ShortcutConfigurationItem>>(pluginCommands);

        if (Shortcuts == null)
            Shortcuts = new();
    }
    public static void SaveShortcuts()
    {
        string data = JsonConvert.SerializeObject(Shortcuts);
        AppSettings.SaveSettings(AppSettingsValues.PluginShortcuts, data);
    }

    public static ShortcutConfigurationItem GetItemFromCommand(ICommand command)
    {
        if (Shortcuts == null)
            return null;

        var uid = PluginHelper.GetUniqueCommandID(command);
        int index = Shortcuts.FindIndex(x => x.UniqueCommandID == uid);
        if (index == -1)
            return null;
        return Shortcuts[index];

    }

    public static string GetShortcutForPlugin(ShortcutConfigurationItem item)
    {
        return string.Join(" + ", item.Keys);
    }

    public static void RemoveShortcut(ICommand clickedCommand)
    {
        var item = GetItemFromCommand(clickedCommand);
        if (item == null)
            return;

        GlobalHotkeyHelper.UnregisterHotkey(item.HotkeyID);
        Shortcuts.Remove(item);

        SaveShortcuts();
        ReregisterAll();
    }

    public static void AddOrUpdate(VirtualKey[] keys, string query, ICommand clickedCommand)
    {
        string uid = PluginHelper.GetUniqueCommandID(clickedCommand);
        int index = Shortcuts.FindIndex(x => x.UniqueCommandID == uid);

        if (index == -1)
            Shortcuts.Add(new ShortcutConfigurationItem { Keys = keys, Query = query, UniqueCommandID = uid });
        else
            Shortcuts[index].Keys = keys;

        SaveShortcuts();
        ReregisterAll();
    }

    public static void RegisterAll()
    {
        for(int i = 0; i < Shortcuts.Count; i++)
        {
            GlobalHotkeyHelper.RegisterHotkey(Shortcuts[i].Keys, (object sender, EventArgs e) =>
            {
                ShortcutConfigurationItem item = (ShortcutConfigurationItem)(e as ObjectEventArgs).Obj;
                if (Callback == null)
                    return;
                ICommand cmd = PluginHelper.GetCommandFromUniqueID(item.UniqueCommandID);
                if (cmd != null)
                    Callback(item.Query, cmd);
            }, out Shortcuts[i].HotkeyID, Shortcuts[i]);
        }
    }

    public static void UnregisterAll()
    {
        for (int i = 0; i < Shortcuts.Count; i++)
            GlobalHotkeyHelper.UnregisterHotkey(Shortcuts[i].HotkeyID);
    }

    public static void ReregisterAll()
    {
        UnregisterAll();
        RegisterAll();
    }

}

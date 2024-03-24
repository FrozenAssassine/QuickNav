using Newtonsoft.Json;
using QuickNav.Core;
using QuickNav.Models;
using QuickNavPlugin;
using System.Collections.Generic;
using Windows.System;

namespace QuickNav.Helper
{
    internal class CommandShortcutHelper
    {
        private static List<ShortcutConfigurationItem> Shortcuts;

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

        public static string GetUniqueCommandID(ICommand command)
        {
            return command.GetType().FullName + command.GetType().Assembly.FullName;
        }

        public static string GetShortcutForPlugin(ICommand command)
        {
            if (Shortcuts == null)
                return "";

            var uid = GetUniqueCommandID(command);
            int index = Shortcuts.FindIndex(x => x.UniqueCommandID == uid);
            if (index == -1)
                return "";

            return string.Join(" + ", Shortcuts[index].Keys);
        }

        public static void AddOrUpdate(VirtualKey[] keys, ICommand clickedCommand)
        {
            string uid = GetUniqueCommandID(clickedCommand);
            int index = Shortcuts.FindIndex(x => x.UniqueCommandID == uid);

            if (index == -1)
                Shortcuts.Add(new ShortcutConfigurationItem { Keys = keys, UniqueCommandID = uid });
            else
                Shortcuts[index].Keys = keys;

            SaveShortcuts();
        }
    }
}

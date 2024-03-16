using QuickNav.Models;
using QuickNavPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.Helper
{
    internal class PluginHelper
    {
        public static List<Plugin> Plugins = new List<Plugin>();

        // TODO: Include priority and amount of command calls into the search order.
        public static List<ICommand> SearchFor(string query)
        {
            List<ICommand> commands = new List<ICommand>();
            string queryLower = query.ToLower();
            string[] keywords = queryLower.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            // Do not combine these loops, as this would destroy the order!
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].TriggerCommands.Count; j++)
                    if (queryLower.StartsWith(Plugins[i].TriggerCommands[j].CommandTrigger.ToLower() + " "))
                        commands.Add(Plugins[i].TriggerCommands[j]);
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].TriggerCommands.Count; j++)
                    for (int k = 0; k < Plugins[i].TriggerCommands[j].Keywords.Length; k++)
                        if (keywords.Contains(Plugins[i].TriggerCommands[j].Keywords[k].ToLower()))
                            commands.Add(Plugins[i].TriggerCommands[j]);
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].CollectorCommands.Count; j++)
                    commands.Add(Plugins[i].CollectorCommands[j]);
            return commands;
        }

        public void InitFromDir(string path)
        {
            string[] files = Directory.GetFiles(path);
            for(int i = 0; i < files.Length; i++)
            {
                if (Path.GetExtension(files[i]).ToLower() == ".dll")
                {
                    Plugin plugin = LoadPluginFromFile(files[i]);
                    if (plugin != null)
                        Plugins.Add(plugin);
                }
            }
        }

        public static Plugin LoadPluginFromFile(string file)
        {
            IPlugin[] interfaces = ReflectionHelper.GetAllExternalInstances(file);
            Plugin plugin = new Plugin();
            for(int i = 0; i < interfaces.Length; i++)
            {
                if (interfaces[i] is IAboutInfo)
                    plugin.Info = (IAboutInfo)interfaces[i];
                if (interfaces[i] is ITriggerCommand)
                    plugin.TriggerCommands.Add((ITriggerCommand)interfaces[i]);
                if (interfaces[i] is IUnknownCommandCollector)
                    plugin.CollectorCommands.Add((IUnknownCommandCollector)interfaces[i]);
            }
            if (plugin.Info != null || plugin.TriggerCommands.Count > 0 || plugin.CollectorCommands.Count > 0)
                return plugin;
            return null;
        }
    }
}

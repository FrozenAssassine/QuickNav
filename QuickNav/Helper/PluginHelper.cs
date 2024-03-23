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
            List<ICommand> triggerMatches = new List<ICommand>();
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].TriggerCommands.Count; j++)
                    if (queryLower.StartsWith(Plugins[i].TriggerCommands[j].CommandTrigger.ToLower()) && Plugins[i].TriggerCommands[j].CommandTrigger != "" && !triggerMatches.Contains(Plugins[i].TriggerCommands[j]))
                        triggerMatches.Add(Plugins[i].TriggerCommands[j]);
            List<ICommand> keywordsMatches = new List<ICommand>();
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].TriggerCommands.Count; j++)
                    for (int k = 0; k < Plugins[i].TriggerCommands[j].Keywords.Length; k++)
                        if (keywords.Contains(Plugins[i].TriggerCommands[j].Keywords[k].ToLower()) && !keywordsMatches.Contains(Plugins[i].TriggerCommands[j]))
                            keywordsMatches.Add(Plugins[i].TriggerCommands[j]);
            List<ICommand> keywordsContains = new List<ICommand>();
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].TriggerCommands.Count; j++)
                    for (int k = 0; k < Plugins[i].TriggerCommands[j].Keywords.Length; k++)
                        for(int l = 0; l < keywords.Length; l++)
                            if (Plugins[i].TriggerCommands[j].Keywords[k].ToLower().Contains(keywords[l].ToLower()) && !keywordsContains.Contains(Plugins[i].TriggerCommands[j]))
                                keywordsContains.Add(Plugins[i].TriggerCommands[j]);
            List<ICommand> unknownCollectors = new List<ICommand>();
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].CollectorCommands.Count; j++)
                    if (!unknownCollectors.Contains(Plugins[i].CollectorCommands[j]))
                        unknownCollectors.Add(Plugins[i].CollectorCommands[j]);

            Comparison<ICommand> comparison = ((ICommand c1, ICommand c2) => { return ((int)c1.Priority(QueryHelper.FixQuery(c1, query))) - (int)c2.Priority(QueryHelper.FixQuery(c2, query)); });
            triggerMatches.Sort(comparison);
            keywordsMatches.Sort(comparison);
            keywordsContains.Sort(comparison);
            unknownCollectors.Sort(comparison);

            var condition = (ICommand cmd) => { return !commands.Contains(cmd); };
            commands.AddRange(triggerMatches.Where(condition));
            commands.AddRange(keywordsMatches.Where(condition));
            commands.AddRange(keywordsContains.Where(condition));
            commands.AddRange(unknownCollectors.Where(condition));

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

        public static List<IFileCommand> GetFilePlugins()
        {
            List<IFileCommand> commands = new List<IFileCommand>();
            for(int i = 0; i < Plugins.Count; i++)
            {
                for (int j = 0; j < Plugins[i].TriggerCommands.Count; j++)
                    if (Plugins[i].TriggerCommands[j] is IFileCommand && !commands.Contains((IFileCommand)Plugins[i].TriggerCommands[j]))
                        commands.Add((IFileCommand)Plugins[i].TriggerCommands[j]);
                for (int j = 0; j < Plugins[i].CollectorCommands.Count; j++)
                    if (Plugins[i].CollectorCommands[j] is IFileCommand && !commands.Contains((IFileCommand)Plugins[i].CollectorCommands[j]))
                        commands.Add((IFileCommand)Plugins[i].CollectorCommands[j]);
            }
            return commands;
        }
    }
}

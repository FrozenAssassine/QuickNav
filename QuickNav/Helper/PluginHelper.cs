using QuickNav.Extensions;
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
    internal static class PluginHelper
    {
        public static List<Plugin> Plugins = new List<Plugin>();

        // TODO: Include priority and amount of command calls into the search order.
        public static List<ICommand> SearchFor(string query)
        {
            List<ICommand> commands = new List<ICommand>();
            string queryLower = query.ToLower();
            string[] keywords = queryLower.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            List<ICommand> triggerFound = new List<ICommand>();
            List<ICommand> keywordFound = new List<ICommand>();
            List<ICommand> keywordContains = new List<ICommand>();
            List<ICommand> other = new List<ICommand>();

            // Do not combine these loops, as this would destroy the order!
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].Commands.Count; j++)
                    if (queryLower.StartsWith(Plugins[i].Commands[j].CommandTrigger.ToLower()) && Plugins[i].Commands[j].CommandTrigger != "")
                        triggerFound.Add(Plugins[i].Commands[j]);
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].Commands.Count; j++)
                    for (int k = 0; k < Plugins[i].Commands[j].Keywords.Length; k++)
                        if (keywords.Contains(Plugins[i].Commands[j].Keywords[k].ToLower()))
                            keywordFound.Add(Plugins[i].Commands[j]);
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].Commands.Count; j++)
                    for (int k = 0; k < Plugins[i].Commands[j].Keywords.Length; k++)
                        for (int l = 0; l < keywords.Length; l++)
                            if (Plugins[i].Commands[j].Keywords[k].ToLower().Contains(keywords[l].ToLower()))
                                keywordContains.Add(Plugins[i].Commands[j]);
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].Commands.Count; j++)
                    if (!triggerFound.Contains(Plugins[i].Commands[j]) && !keywordFound.Contains(Plugins[i].Commands[j]) && !keywordContains.Contains(Plugins[i].Commands[j]))
                        other.Add(Plugins[i].Commands[j]);

            triggerFound.SortByPriority(query);
            keywordFound.SortByPriority(query);
            keywordContains.SortByPriority(query);
            other.SortByPriority(query);

            var condition = (ICommand cmd) => { return !commands.Contains(cmd); };
            commands.AddRange(triggerFound.Where(condition));
            commands.AddRange(keywordFound.Where(condition));
            commands.AddRange(keywordContains.Where(condition));
            commands.AddRange(other.Where(condition));

            // Do not combine these loops, as this would destroy the order!
            /*List<ICommand> triggerMatches = new List<ICommand>();
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

            triggerMatches.SortByPriority(query);
            keywordsMatches.SortByPriority(query);
            keywordsContains.SortByPriority(query);
            unknownCollectors.SortByPriority(query);

            var condition = (ICommand cmd) => { return !commands.Contains(cmd); };
            commands.AddRange(triggerMatches.Where(condition));
            commands.AddRange(keywordsMatches.Where(condition));
            commands.AddRange(keywordsContains.Where(condition));
            commands.AddRange(unknownCollectors.Where(condition));*/

            return commands;
        }

        public static string GetUniqueCommandID(ICommand command)
        {
            return command.GetType().FullName + command.GetType().Assembly.FullName;
        }

        public static ICommand GetCommandFromUniqueID(string uniqueID)
        {
            for (int i = 0; i < Plugins.Count; i++)
                for (int j = 0; j < Plugins[i].Commands.Count; j++)
                    if (GetUniqueCommandID(Plugins[i].Commands[j]) == uniqueID)
                        return Plugins[i].Commands[j];
            return null;
        }

        public static void InitFromDir(string path)
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
                if (interfaces[i] is ICommand)
                    plugin.Commands.Add((ICommand)interfaces[i]);
            }
            if (plugin.Info != null || plugin.Commands.Count > 0)
                return plugin;
            return null;
        }

        public static List<IFileCommand> GetFilePlugins()
        {
            List<IFileCommand> commands = new List<IFileCommand>();
            for(int i = 0; i < Plugins.Count; i++)
            {
                for (int j = 0; j < Plugins[i].Commands.Count; j++)
                    if (Plugins[i].Commands[j] is IFileCommand)
                        commands.Add((IFileCommand)Plugins[i].Commands[j]);
            }
            return commands;
        }

        public static List<ITextCommand> GetTextPlugins()
        {
            List<ITextCommand> commands = new List<ITextCommand>();
            for (int i = 0; i < Plugins.Count; i++)
            {
                for (int j = 0; j < Plugins[i].Commands.Count; j++)
                    if (Plugins[i].Commands[j] is ITextCommand)
                        commands.Add((ITextCommand)Plugins[i].Commands[j]);
            }
            return commands;
        }
    }
}

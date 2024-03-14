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

using QuickNav.Helper;
using QuickNav.Models;
using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.InstallPluginCommand
{
    internal class InstallPluginCommand : IFileCommand
    {
        public string[] ExtensionFilter => new string[] { "dll" };

        public bool AcceptMultipleFiles => true;

        public string Description => "Drop plugin as DLL to install it!";

        public string CommandTrigger => "plugin:";

        public string[] Keywords => new string[] { "plugin", "install", "extension" };

        public Uri Icon(string query) => new Uri("ms-appx://App/Assets/commands/wordcounter.png"); // TODO: add correct image

        public string Name(string query)
        {
            return "Install dll as plugin";
        }

        public void OnWindowOpened()
        {
            
        }

        static bool IsExeDll(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            return extension.Equals(".exe", StringComparison.OrdinalIgnoreCase) ||
                   extension.Equals(".dll", StringComparison.OrdinalIgnoreCase);
        }

        public Priority Priority(string query)
        {
            if (query == "")
                return QuickNavPlugin.Priority.Invisible;
            string fn = query.Trim().Trim('\"');
            if (File.Exists(fn) && IsExeDll(fn))
            {
                IPlugin[] plugins = ReflectionHelper.GetAllExternalInstances(fn);
                if (plugins.Length > 0)
                    return QuickNavPlugin.Priority.High;
            }
            return QuickNavPlugin.Priority.Invisible;
        }

        public bool RunCommand(string file, out ContentElement content)
        {
            content = null;

            file = file.Trim().Trim('\"');

            if (!File.Exists(file) || !IsExeDll(file))
            {
                //TODO show error that file does not exist
                return false;
            }

            Plugin plugin = PluginHelper.LoadPluginFromFile(file);
            if (plugin.Commands.Count > 0 || plugin.Services.Count > 0)
            {
                PluginHelper.Plugins.Add(plugin);
                string dest = Path.Combine(MainWindow.ExtensionPath, Path.GetFileName(file));
                if (File.Exists(dest))
                {
                    // TODO: fix path because of duplicate filename
                }
                File.Copy(file, dest);
            }

            return true;
        }
    }
}

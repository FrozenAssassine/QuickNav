using Microsoft.UI.Xaml.Controls;
using QuickNav.Models;
using QuickNav.Views;
using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.ConverterCommand
{
    internal class ConverterCommand : IFileCommand, IBuildInCommand
    {
        public string[] ExtensionFilter => new string[0];

        public bool AcceptMultipleFiles => true;

        public string Description => "Converts files between different file types.";

        public string CommandTrigger => "convert:";

        public string[] Keywords => new string[] { "convert", "file", "type", "extension" };

        public Uri Icon(string query) => new Uri("ms-appx://App/Assets/commands/convert.png");

        public string Name(string query)
        {
            if(query.Length == 0)
                return "Convert files ...";

            string firstfile = query.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0];
            if (File.Exists(firstfile))
                return "Convert *" + Path.GetExtension(firstfile) + " to ...";

            return "Convert files ...";
        }

        public void OnWindowOpened()
        {
            
        }

        public Priority Priority(string query)
        {
            string[] files = query.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            bool exists = true;
            bool inputexists = true;
            for (int i = 0; i < files.Length; i++)
                if (!File.Exists(files[i]))
                    exists = false;
                else
                    if (!ConverterRegistry.InputExistsFor(files[i]))
                        inputexists = false;
            if (exists && inputexists)
                return QuickNavPlugin.Priority.High;
            //if (exists)
            //    return QuickNavPlugin.Priority.Low;
            return QuickNavPlugin.Priority.Invisible;
        }

        public bool RunCommand(string parameters, out ContentElement content)
        {
            content = null;
            return false;
        }

        public bool RunCommand(string parameters, out Page content, out double addWidth, out double addHeight)
        {
            string[] files = parameters.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            addWidth = 0;
            addHeight = 0;
            content = new ConverterView(files);
            return true;
        }
    }
}

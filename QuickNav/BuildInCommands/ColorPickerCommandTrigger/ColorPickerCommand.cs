using Microsoft.UI.Xaml.Controls;
using QuickNav.Models;
using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.ColorPickerCommandTrigger
{
    public class ColorPickerCommand : ICommand, IBuildInCommand
    {
        public string CommandTrigger => "clr";

        public string[] Keywords => new string[] { "color", "picker" };

        public string Description => "Pick a color from color circle.";

        public Uri Icon => new Uri("ms-appx://App/Assets/commands/colorpicker.png");

        public Priority Priority(string query)
        {
            return QuickNavPlugin.Priority.Low;
        }

        public string Name(string query)
        {
            return "Color Picker";
        }

        public bool RunCommand(string parameters, out ContentElement content)
        {
            content = null;
            return false;
        }

        public bool RunCommand(string parameters, out Page content, out double width, out double height)
        {
            content = new QuickNav.Views.ColorPicker();
            width = 0;
            height = 280;
            return true;
        }

        public void OnWindowOpened() { }
    }
}

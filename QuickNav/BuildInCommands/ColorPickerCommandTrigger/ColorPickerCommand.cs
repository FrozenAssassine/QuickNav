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
    public class ColorPickerCommand : ITriggerCommand, IBuildInCommand
    {
        public string CommandTrigger => "";

        public string[] Keywords => new string[] { "color", "picker" };

        public string Description => "Pick a color from color circle.";

        public Uri Icon => new Uri("ms-appx://App/Assets/commands/colorpicker.png");

        public Priority Priority => Priority.Low;

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
            width = -150;
            height = 250;
            return true;
        }
    }
}

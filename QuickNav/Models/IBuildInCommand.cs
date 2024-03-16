using Microsoft.UI.Xaml.Controls;
using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.Models
{
    internal interface IBuildInCommand : ICommand
    {
        bool RunCommand(string parameters, out Page content);
    }
}

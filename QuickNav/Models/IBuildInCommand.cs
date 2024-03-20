using Microsoft.UI.Xaml.Controls;
using QuickNavPlugin;

namespace QuickNav.Models
{
    internal interface IBuildInCommand : ICommand
    {
        bool RunCommand(string parameters, out Page content, out double addWidth, out double addHeight);
    }
}

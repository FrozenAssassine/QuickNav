using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickNavPlugin.UI;

namespace QuickNavPlugin
{
    public interface ICommand : IPlugin
    {
        /// <summary>
        /// Describes the functionality of the command.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Icon for the command.
        /// </summary>
        Uri Icon { get; }
        /// <summary>
        /// Priority of the command, should it be shown on top or at bottom?
        /// </summary>
        Priority Priority { get; }
        string Name(string query);
        /// <summary>
        /// This function is called to run the command.
        /// </summary>
        /// <param name="parameters">The parameters of the command.</param>
        /// <param name="content">The result, shown to the user.</param>
        /// <returns>Returns true or false, whether execution were successfully or not.</returns>
        bool RunCommand(string parameters, out ContentElement content);
    }
}

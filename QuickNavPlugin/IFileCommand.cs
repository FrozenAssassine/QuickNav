using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin
{
    /// <summary>
    /// You can use this interface to assign files to your command using drag & drop.
    /// </summary>
    public interface IFileCommand : ICommand
    {
        /// <summary>
        /// Filter for file extension (e.g. "txt" for *.txt files), leave empty to accept any file type; Write all the extensions in lower case!
        /// </summary>
        string[] ExtensionFilter { get; }
    }
}

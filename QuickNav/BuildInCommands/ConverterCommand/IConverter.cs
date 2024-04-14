using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands.ConverterCommand
{
    public interface IConverter
    {
        /// <summary>
        /// Types of input files (must be lower case!), e.g. "txt" for text-files, "png" for images, etc.
        /// </summary>
        string[] InputTypes { get; }
        /// <summary>
        /// Types of output files (must be lower case!), e.g. "txt" for text-files, "png" for images, etc.
        /// </summary>
        string[] OutputTypes { get; }
        /// <summary>
        /// This method will be called to convert files.
        /// </summary>
        /// <param name="paths">The file paths of the files that should be converted.</param>
        /// <param name="OutputType">The selected output file type e.g. "png", "txt", etc.</param>
        /// <returns>The file paths of the converted files. The converted files should have the same filenames and order.</returns>
        string[] ConvertTo(string[] paths, string OutputType);
    }
}

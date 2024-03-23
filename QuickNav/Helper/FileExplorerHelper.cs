using System.Diagnostics;
using System.IO;
using System.Text;

namespace QuickNav.Helper;

internal class FileExplorerHelper
{
    public static void OpenExplorer(string fullPath)
    {
        Process.Start("explorer.exe", $"/select,\"{fullPath}\"");
    }
}

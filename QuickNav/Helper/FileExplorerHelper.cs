using System.Diagnostics;
using System.IO;

namespace QuickNav.Helper;

internal class FileExplorerHelper
{
    public static void OpenExplorer(string fullPath)
    {
        string folderPath = Path.GetDirectoryName(fullPath);

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "explorer.exe",
            Arguments = folderPath
        };

        if (File.Exists(fullPath))
        {
            psi.Arguments += $" /select,\"{fullPath}\"";
        }

        Process.Start(psi);
    }
}

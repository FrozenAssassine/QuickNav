using System.Diagnostics;
using System.IO;
using System.Text;

namespace QuickNav.Helper;

internal class FileExplorerHelper
{
    public static void OpenExplorer(string fullPath)
    {
        byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(fullPath);
        string englishPath = Encoding.UTF8.GetString(bytes);

        Process.Start("explorer.exe", $"\"{englishPath}\"");
    }
}

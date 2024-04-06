using System.Diagnostics;

namespace QuickNav.Helper;

internal class FileExplorerHelper
{
    public static void OpenExplorer(string fullPath)
    {
        Process.Start("explorer.exe", $"/select,\"{fullPath.Trim()}\"");
    }

    public static string FileSize(long size)
    {
        if (size < 1_000)
            return size + "B";
        else if (size > 1_000 && size < 1_000_000)
            return (size / 1_000) + "KB";
        else if (size > 1_000_000 && size < 1_000_000_000)
            return (size / 1_000_000) + "MB";
        else if (size > 1_000_000_000)
            return (size / 1_000_000_000) + "GB";
        return "";
    }
}

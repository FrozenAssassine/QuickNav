namespace QuickNav.Models;

internal class FileSearchResultItem
{
    public FileSearchResultItem(string fileName, string filePath)
    {
        FileName = fileName;
        FilePath = filePath;
    }

    public string FileName { get; set; }
    public string FilePath { get; set; }
}

using System.Collections.Generic;

namespace QuickNavPlugin.UI;

public class SearchedFilesViewElement : ContentElement
{
    public List<(string name, string path)> Files { get; } = new();
}

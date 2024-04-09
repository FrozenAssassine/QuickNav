using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.IO;
using QuickNav.Extensions;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace QuickNav.BuildInCommands.FileInfoCommandCollector;

internal class CountWordsCommand : ICommand, IFileCommand
{
    public string Description => "Count the words in a file or clipboard";

    public Uri Icon => new Uri("ms-appx://App/Assets/commands/wordcounter.png");

    public Priority Priority(string query)
    {
        if (query == "")
            return QuickNavPlugin.Priority.Invisible;
        if (File.Exists(query.Trim().Trim('\"')) && ExtensionFilter.Contains(Path.GetExtension(query).Substring(1)))
            return QuickNavPlugin.Priority.Medium;
        return QuickNavPlugin.Priority.Invisible;
    }

    public string CommandTrigger => "count:";

    public string[] Keywords => new string[] { "count", "word"};

    public string[] ExtensionFilter => new string[] { "txt", "md", "rtf", "html", "xml", "csv", "json", "log", "yaml", "ini", "conf", "css", "scss", "js", "jsx", "ts", "tsx", "c", "cpp", "h", "hpp", "java", "py", "rb", "php", "pl", "sh", "bat", "ps1", "sql", "asm" };

    public string Name(string query)
    {
        return "Count Words";
    }

    public bool RunCommand(string param, out ContentElement content)
    {
        content = null;

        param = param.Trim().Trim('\"');
        if (!File.Exists(param))
            return false;

        string text;
        //no file, count the clipboard
        if(param.Length == 0)
        {
            text = System.Windows.Clipboard.GetText();
        }
        else
        {
            if (!File.Exists(param))
                text = param;
            else
                text = File.ReadAllText(param);
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("## Words: " + text.CountWords());
        sb.AppendLine("## Characters: " + text.Length);
        sb.AppendLine("## Lines: " + text.CountLines());
        sb.AppendLine("## Sentences: " + text.CountSentences());
        sb.AppendLine("## Paragraphs: " + text.CountParagraphs());
        content = new MarkdownElement(sb.ToString());
        return true;
    }

    public void OnWindowOpened() { }
}
using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.IO;
using QuickNav.Extensions;

namespace QuickNav.BuildInCommands.FileInfoCommandCollector;

internal class CountWordsCommand : ITriggerCommand, IUnknownCommandCollector, IFileCommand
{
    public string Description => "Count the words in a file or clipboard";

    public Uri Icon => new Uri("ms-appx://App/Assets/commands/wordcounter.png");

    public Priority Priority => Priority.Low;

    public string CommandTrigger => "count:";

    public string[] Keywords => new string[] { "count", "word"};

    public string[] ExtensionFilter => new string[] { "txt", "md", "rtf" };

    public string Name(string query)
    {
        return "Count Words";
    }

    public bool RunCommand(string param, out ContentElement content)
    {
        var textLabel = new LabelElement();
        content = textLabel;

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

        textLabel.Text = "Words: " + text.CountWords() + "\nCharacter: " + text.Length + "\nLines: " + text.CountLines();

        return true;
    }
}
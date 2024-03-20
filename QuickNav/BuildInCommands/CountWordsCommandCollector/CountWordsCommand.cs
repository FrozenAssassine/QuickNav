using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.IO;

namespace QuickNav.BuildInCommands.FileInfoCommandCollector;

internal class CountWordsCommand : ITriggerCommand, IUnknownCommandCollector
{
    public string Description => "Count the words in a file or clipboard";

    public Uri Icon => new Uri("ms-appx://App/Assets/commands/wordcounter.png");

    public Priority Priority => Priority.Low;

    public string CommandTrigger => "cnt:";

    public string[] Keywords => new string[] { "count", "word"};

    public string Name(string query)
    {
        return "Count Words";
    }
    private static int CountWords(string str)
    {
        return str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    static int CountLines(string text)
    {
        if (text.Contains("\r\n"))
        {
            return text.Split("\r\n").Length;
        }
        return text.Split("\n").Length;
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

        textLabel.Text = "Words: " + CountWords(text) + "\nCharacter: " + text.Length + "\nLines: " + CountLines(text);

        return true;
    }
}
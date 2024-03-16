using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.IO;

namespace QuickNav.BuildInCommands.FileInfoCommandCollector;

internal class CountWordsCommand : ITriggerCommand
{
    public string Description => "Count the words in a file or clipboard";

    public Uri Icon => null;

    public Priority Priority => Priority.Low;

    public string CommandTrigger => "cnt:";

    public string[] Keywords => new string[] { "count", "words"};

    public string Name(string query)
    {
        return "Count Words";
    }
    private static int CountWords(string str)
    {
        int index = -1;
        int count = 0;
        while (-1 != (index = str.IndexOf('\n', index + 1)))
            count++;
        return count + 1;
    }

    static int CountLines(string text)
    {
        if (text.Contains("\r\n"))
        {
            return text.Split("\r\n").Length;
        }
        return text.Split("\n").Length;
    }

    public bool RunCommand(string file, out ContentElement content)
    {
        var textLabel = new LabelElement();
        content = textLabel;

        string text = File.ReadAllText(file);

        textLabel.Text = "Words: " + CountWords(text) + "\nCharacter: " + text.Length + "\nLines: " + CountLines(text);

        return true;
    }
}
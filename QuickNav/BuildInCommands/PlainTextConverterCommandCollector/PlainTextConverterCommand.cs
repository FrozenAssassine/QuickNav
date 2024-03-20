using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;

namespace QuickNav.BuildInCommands.PlainTextConverterCommandCollector;

internal class PlainTextConverterCommand : ITriggerCommand
{
    public string Description => "Convert text in your clipboard to plain text";

    public Uri Icon => new Uri("ms-appx://App/Assets/commands/clipboardtext.png");

    public Priority Priority => Priority.Low;

    public string CommandTrigger => "pt"; //plain text

    public string[] Keywords => new string[] { "plain text", "text" };

    public string Name(string query)
    {
        return "Convert Clipboard to plain text";
    }

    public bool RunCommand(string file, out ContentElement content)
    {
        string text = System.Windows.Clipboard.GetText();

        System.Windows.Clipboard.SetText(text);

        content = null;

        return true;
    }
}
using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace QuickNav.BuildInCommands.PlainTextConverterCommandCollector;

internal class PlainTextConverterCommand : ICommand
{
    public string Description => "Convert text in your clipboard to plain text";

    public Uri Icon(string query) => new Uri("ms-appx://App/Assets/commands/clipboardtext.png");

    public Priority Priority(string query)
    {
        return QuickNavPlugin.Priority.Low;
    }

    public string CommandTrigger => "pt"; //plain text

    public string[] Keywords => new string[] { "plain", "text" };

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

    public void OnWindowOpened() { }
}
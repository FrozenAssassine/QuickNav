namespace QuickNavPlugin.UI;

public class MarkdownElement : ContentElement
{
    public MarkdownElement(string markdown)
    {
        this.Markdown = markdown;
    }
    public string Markdown { get; set; }
}

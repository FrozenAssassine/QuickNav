namespace QuickNavPlugin.UI;

public class MarkdownElement : ContentElement
{
    private string _Markdown;
    public string Markdown
    {
        get => _Markdown;
        set
        {
            _Markdown = value;
            if (TextChanged != null) TextChanged(this, value);
        }
    }

    public ElementTextChanged TextChanged;

    public MarkdownElement(string markdown = "")
    {
        _Markdown = markdown;
    }
}

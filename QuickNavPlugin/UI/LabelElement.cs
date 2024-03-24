namespace QuickNavPlugin.UI;

/// <summary>
/// Represents a label.
/// </summary>
public class LabelElement : ContentElement
{
    private bool _Scrollable = false;
    public bool Scrollable
    {
        get => _Scrollable;
        set
        {
            _Scrollable = value;
            if (ScrollableChanged != null) ScrollableChanged(this, value);
        }
    }
    public bool AutoScrollBottom { get; set; } = false;
    private string _Text;
    public string Text
    {
        get => _Text;
        set
        {
            _Text = value;
            if (TextChanged != null) TextChanged(this, value);
        }
    }

    public ElementTextChanged TextChanged;
    public ElementIsEditableChanged IsEditableChanged;
    public ElementScrollableChanged ScrollableChanged;

    public LabelElement(string text = "")
    {
        _Text = text;
    }
}

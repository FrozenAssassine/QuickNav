using ABI.Microsoft.UI.Xaml;

namespace QuickNavPlugin.UI;

/// <summary>
/// Represents a label.
/// </summary>
public class LabelElement : ContentElement
{
    public LabelElement(string text = "")
    {
        this.Text = text;
    }
    public bool Scrollable { get; set; } = false;
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
}

namespace QuickNavPlugin.UI;

/// <summary>
/// Represents a Progressbar.
/// </summary>
public class ProgressBarElement : ContentElement
{
    public string Header { get; set; }
    public double Width { get; set; }
    private double _progress;
    public double Progress 
    {
        get => _progress;
        set
        {
            _progress = value;
            TextChanged(this, _progress.ToString());
        }
    }
    public ElementTextChanged TextChanged;
}

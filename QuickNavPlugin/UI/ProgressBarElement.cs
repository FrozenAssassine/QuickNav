namespace QuickNavPlugin.UI;

/// <summary>
/// Represents a Progressbar.
/// </summary>
public class ProgressBarElement : ContentElement
{
    private double _progress;
    public double Progress 
    {
        get => _progress;
        set
        {
            _progress = value;
            if (ProgressChanged != null) ProgressChanged(this, _progress);
        }
    }
    public ElementProgressChanged ProgressChanged;
}

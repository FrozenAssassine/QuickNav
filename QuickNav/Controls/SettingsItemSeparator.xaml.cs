using Microsoft.UI.Xaml.Controls;

namespace QuickNav.Controls;

public sealed partial class SettingsItemSeparator : UserControl
{
    public SettingsItemSeparator()
    {
        this.InitializeComponent();
    }

    public new object Content { get; set; }

    public string Header { get; set; }
}

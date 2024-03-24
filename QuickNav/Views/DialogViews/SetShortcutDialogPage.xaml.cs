using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.System;

namespace QuickNav.Views.DialogViews;

public sealed partial class SetShortcutDialogPage : Page
{
    public HashSet<VirtualKey> pressedKeys = new HashSet<VirtualKey>();
    public SetShortcutDialogPage()
    {
        this.InitializeComponent();
    }

    private void Page_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (queryInput.FocusState == Microsoft.UI.Xaml.FocusState.Unfocused)
        {
            pressedKeys.Add(e.Key);
            UpdateShortcutDisplay();
        }
    }

    public string GetQuery => queryInput.Text;

    public void UpdateShortcutDisplay()
    {
        ShortcutDisplay.Content = "Shortcut: ";

        if (pressedKeys.Count == 0)
            return;

        foreach (var key in pressedKeys.SkipLast(1))
        {
            ShortcutDisplay.Content += key.ToString() + "+";
        }
        ShortcutDisplay.Content += pressedKeys.Last().ToString();

    }
}

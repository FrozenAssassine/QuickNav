using Microsoft.UI.Xaml;
using QuickNav.Helper;
using QuickNavPlugin;

namespace QuickNav.Models
{
    class ResultListViewItem
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
        public string Shortcut { get => CommandShortcutHelper.GetShortcutForPlugin(Command); }
        public Visibility ShortcutVisibility { get => ConvertHelper.BoolToVisibility(Shortcut.Length != 0); }
    }
}

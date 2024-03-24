using Microsoft.UI.Xaml;
using QuickNav.Helper;
using QuickNavPlugin;

namespace QuickNav.Models
{
    class ResultListViewItem
    {
        public string Text { get; set; }
        private ICommand _Command;
        public ICommand Command { get => _Command; set { _Command = value; shortcutItem = CommandShortcutHelper.GetItemFromCommand(value); } }
        public string Shortcut { get => shortcutItem == null ? "" : CommandShortcutHelper.GetShortcutForPlugin(shortcutItem); }
        public Visibility ShortcutVisibility { get => ConvertHelper.BoolToVisibility(Shortcut.Length != 0); }
        public string Query { get => shortcutItem == null ? "" : shortcutItem.Query; }

        private ShortcutConfigurationItem shortcutItem;
    }
}

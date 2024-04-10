using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using QuickNav.Helper;
using QuickNavPlugin;
using System;

namespace QuickNav.Models
{
    class ResultListViewItem
    {
        public string Text { get; set; }
        public ImageSource Uri { get; set; }
        private ICommand _Command;
        public ICommand Command { get => _Command; set { _Command = value; shortcutItem = CommandShortcutHelper.GetItemFromCommand(value); autostartItem = CommandAutostartHelper.GetItemFromCommand(value); } }
        
        public string Shortcut { get => shortcutItem == null ? "" : CommandShortcutHelper.GetShortcutForPlugin(shortcutItem); }
        public Visibility ShortcutVisibility { get => ConvertHelper.BoolToVisibility(Shortcut.Length != 0); }
        public string ShortcutQuery { get => shortcutItem == null ? "" : shortcutItem.Query; }
        
        public string Autostart { get => autostartItem == null ? "" : "Reboot"; }
        public Visibility AutostartVisibility { get => ConvertHelper.BoolToVisibility(Autostart.Length != 0); }
        public string AutostartQuery { get => autostartItem == null ? "" : autostartItem.Query; }

        private ShortcutConfigurationItem shortcutItem;
        private AutostartConfigurationItem autostartItem;
    }
}

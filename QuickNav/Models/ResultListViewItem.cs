using Microsoft.UI.Xaml;
using QuickNavPlugin;

namespace QuickNav.Models
{
    class ResultListViewItem
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
    }
}

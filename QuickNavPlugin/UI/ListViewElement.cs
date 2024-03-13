using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin.UI
{
    public class ListViewElement : ContentElement
    {
        private Orientation _Orientation;
        public Orientation Orientation
        {
            get => _Orientation;
            set
            {
                _Orientation = value;
                if (OrientationChanged != null) OrientationChanged(this, value);
            }
        }
        private ObservableCollection<ContentElement> _Children;
        public ObservableCollection<ContentElement> Children => _Children;

        public ElementOrientationChanged OrientationChanged;
        public ElementChildrenChanged ChildrenChanged;

        public ListViewElement()
        {
            _Children = new ObservableCollection<ContentElement>();
            _Children.CollectionChanged += _Children_CollectionChanged;
        }

        private void _Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (OrientationChanged != null) ChildrenChanged(this, _Children);
        }
    }
}

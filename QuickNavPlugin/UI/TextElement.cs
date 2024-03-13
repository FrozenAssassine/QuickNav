using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin.UI
{
    public class TextElement : ContentElement
    {
        private string _Text;
        public string Text
        {
            get => _Text;
            set
            {
                _Text = value;
                if(TextChanged != null) TextChanged(this, value);
            }
        }
        private bool _IsEditable;
        public bool IsEditable
        {
            get => _IsEditable;
            set
            {
                if(IsEditableChanged != null) IsEditableChanged(this, value);
            }
        }

        public ElementTextChanged TextChanged;
        public ElementIsEditableChanged IsEditableChanged;
    }
}

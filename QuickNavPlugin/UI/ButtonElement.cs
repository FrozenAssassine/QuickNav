using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin.UI
{
    public class ButtonElement : ContentElement
    {
        private string _Text;
        public string Text
        {
            get => _Text;
            set
            {
                _Text = value;
                if (TextChanged != null) TextChanged(this, value);
            }
        }

        public ElementTextChanged TextChanged;
    }
}

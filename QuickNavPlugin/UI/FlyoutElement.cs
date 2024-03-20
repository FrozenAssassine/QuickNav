using System;
using System.Collections.Generic;

namespace QuickNavPlugin.UI;

public class FlyoutElement
{
    public List<ContentElement> Items = new List<ContentElement>();

    public void AddButton(string text, ElementClicked clicked)
    {
        Items.Add(new ButtonElement() { Text = text, Clicked = clicked });
    }
}

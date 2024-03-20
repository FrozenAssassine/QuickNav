using System;
using System.Collections.Generic;

namespace QuickNavPlugin.UI;

public class MenuFlyoutElement: FlyoutBaseElement
{
    public List<MenuFlyoutItemElement> Items = new();

    public void AddItem(string text, string glyph, Action<MenuFlyoutItemElement> clicked)
    {
        Items.Add(new MenuFlyoutItemElement
        {
            Clicked = clicked,
            Text = text,
            Glyph = glyph
        });
    }
}

public class MenuFlyoutItemElement
{
    public object Tag { get; set; }
    public string Text { get; set; }
    public string Glyph { get; set; }
    public Action<MenuFlyoutItemElement> Clicked { get; set; }
}

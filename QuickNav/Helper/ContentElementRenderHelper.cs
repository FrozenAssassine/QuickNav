using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Input;
using QuickNavPlugin.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using System.Net.Mime;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace QuickNav.Helper
{
    internal class ContentElementRenderHelper
    {
        public static FlyoutBase CreateFlyout(FlyoutElement flyout)
        {
            ListViewElement lv = new ListViewElement();
            lv.Orientation = QuickNavPlugin.UI.Orientation.Vertical;
            foreach (ContentElement ce in flyout.Items)
                lv.Children.Add(ce);

            Flyout f = new Flyout();
            f.Content = RenderContentElement(lv);
            return f;
        }

        public static UIElement RenderContentElement(ContentElement content)
        {
            if(content is ButtonElement buttonElement)
            {
                Button btn = new Button();
                btn.Content = buttonElement.Text;
                if (content.Flyout != null) btn.ContextFlyout = CreateFlyout(content.Flyout);
                btn.Click += (object sender, RoutedEventArgs e) =>
                {
                    if (buttonElement.Clicked != null) buttonElement.Clicked(buttonElement);
                };
                buttonElement.TextChanged += (ContentElement sender, string text) =>
                {
                    btn.Content = text;
                };
                return btn;
            }
            if(content is ImageElement imageElement)
            {
                Image img = new Image();
                img.Source = new BitmapImage(imageElement.Image);
                img.PointerPressed += (object sender, PointerRoutedEventArgs e) =>
                {
                    e.Handled = true;
                    PointerPoint ptrPt = e.GetCurrentPoint(img);
                    if (imageElement.ClickedPosition != null) imageElement.ClickedPosition(imageElement, ptrPt.Position.X, ptrPt.Position.Y);
                    if (imageElement.Clicked != null) imageElement.Clicked(imageElement);
                };
                imageElement.ImageChanged += (ContentElement sender, Uri source) =>
                {
                    img.Source = new BitmapImage(source);
                };
                return img;
            }
            if(content is ListViewElement listElement)
            {
                void SetOrientation(StackPanel sp, QuickNavPlugin.UI.Orientation orientation) =>
                    sp.Orientation = orientation == QuickNavPlugin.UI.Orientation.Horizontal ?
                        Microsoft.UI.Xaml.Controls.Orientation.Horizontal :
                        Microsoft.UI.Xaml.Controls.Orientation.Vertical;

                ScrollViewer scrollviewer = new ScrollViewer();
                scrollviewer.VerticalAlignment = VerticalAlignment.Top;
                scrollviewer.VerticalScrollMode = ScrollMode.Enabled;
                scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                StackPanel stack = new StackPanel();
                scrollviewer.Content = stack;
                SetOrientation(stack, listElement.Orientation);


                for (int i = 0; i < listElement.Children.Count; i++)
                    stack.Children.Add(RenderContentElement(listElement.Children[i]));
                listElement.OrientationChanged += (ContentElement sender, QuickNavPlugin.UI.Orientation orientation) =>
                {
                    SetOrientation(stack, orientation);
                };
                listElement.ChildrenChanged += (ContentElement sender, IEnumerable<ContentElement> children) =>
                {
                    stack.Children.Clear();
                    foreach (ContentElement child in children)
                        stack.Children.Add(RenderContentElement(child));
                };
                return scrollviewer;
            }
            if(content is TextElement textElement)
            {
                TextBox textBox = new TextBox();
                if (content.Flyout != null) textBox.ContextFlyout = CreateFlyout(content.Flyout);
                textBox.Text = textElement.Text;
                textBox.IsReadOnly = !textElement.IsEditable;
                textBox.BorderThickness = new Thickness(0);
                textBox.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                ElementTextChanged textChanged = (ContentElement sender, string Text) =>
                {
                    textBox.Text = Text;
                };
                textElement.TextChanged += textChanged;
                textElement.IsEditableChanged += (ContentElement sender, bool isEditable) =>
                {
                    textBox.IsReadOnly = !isEditable;
                    if (isEditable)
                        textBox.BorderThickness = new Thickness(1);
                    else
                        textBox.BorderThickness = new Thickness(0);
                };
                textBox.PointerPressed += (object sender, PointerRoutedEventArgs e) =>
                {
                    if (textElement.Clicked != null) textElement.Clicked(textElement);
                };
                textBox.TextChanged += (object sender, TextChangedEventArgs e) =>
                {
                    textElement.TextChanged -= textChanged;
                    textElement.Text = textBox.Text;
                    if (textElement.TextChanged != null) textElement.TextChanged(textElement, textBox.Text);
                    textElement.TextChanged += textChanged;
                };
                return textBox;
            }
            if (content is LabelElement labelElement)
            {
                ScrollView sv = new ScrollView();
                TextBlock textBlock = new TextBlock();
                textBlock.Text = labelElement.Text;
                textBlock.IsTextSelectionEnabled = true;
                if (content.Flyout != null) textBlock.ContextFlyout = CreateFlyout(content.Flyout);
                textBlock.PointerPressed += (object sender, PointerRoutedEventArgs e) =>
                {
                    if (labelElement.Clicked != null) labelElement.Clicked(labelElement);
                };
                labelElement.TextChanged += (ContentElement sender, string Text) =>
                {
                    textBlock.Text = Text;
                    if (labelElement.AutoScrollBottom)
                    {
                        sv.ScrollTo(0, sv.ScrollableHeight);
                    }
                };

                if (labelElement.Scrollable)
                {
                    sv.Content = textBlock;
                    return sv;
                }

                return textBlock;
            }
            return null;
        }
    }
}

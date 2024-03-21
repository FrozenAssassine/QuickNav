// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using QuickNav.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuickNav.Controls
{
    public sealed partial class SettingsControl : UserControl
    {
        public delegate void ClickedEvent(SettingsControl sender);
        public event ClickedEvent Clicked;

        public SettingsControl()
        {
            this.InitializeComponent();
        }

        public bool Clickable { get; set; }

        private string _Glyph;
        public string Glyph { get => _Glyph; set { _Glyph = value; iconDisplay.Visibility = ConvertHelper.BoolToVisibility(value.Length > 0); } }
        public string Header { get; set; }
        public new UIElement Content
        {
            set
            {
                contentHost.Content = value;
            }
        }

        private void mainGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Clicked?.Invoke(this);
        }

        private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!Clickable)
                return;

            EnterStoryboard.Begin();
        }

        private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!Clickable)
                return;

            ExitStoryboard.Begin();
        }
    }
}
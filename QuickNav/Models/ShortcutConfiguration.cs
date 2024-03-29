﻿using System.Windows.Input;
using Windows.System;

namespace QuickNav.Models;

internal class ShortcutConfigurationItem
{
    public string UniqueCommandID { get; set; }
    public VirtualKey[] Keys { get; set; }
    public int HotkeyID;
    public string Query { get; set; }
}

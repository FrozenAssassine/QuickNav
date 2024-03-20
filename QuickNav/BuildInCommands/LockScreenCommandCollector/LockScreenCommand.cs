using QuickNavPlugin;
using System;
using QuickNav.Helper;

namespace QuickNav.BuildInCommands.LockScreenCommandCollector;

internal class LockScreenCommand : IUnknownCommandCollector, ITriggerCommand
{
    public string Description => "Run this to lock your screen";

    public Uri Icon => null; //TODO FOR finn

    public Priority Priority => Priority.Low;

    public string CommandTrigger => "lock";

    public string[] Keywords => new string[] { "lock", "screen"};

    public string Name(string query)
    {
        return "Lock your screen";
    }

    public bool RunCommand(string searchTerm, out QuickNavPlugin.UI.ContentElement content)
    {
        content = null;

        Win32Apis.LockWorkStation();

        return true;
    }
}
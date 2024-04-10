using QuickNav.Helper;
using QuickNavPlugin;
using System;
using System.Collections.Generic;

namespace QuickNav.Extensions;

public static class CommandListExtension
{
    public static void SortByPriority(this List<ICommand> commands, string query)
    {
        List<int> priorities = new List<int>();
        for(int i = 0; i < commands.Count; i++)
        {
            Priority p = commands[i].Priority(QueryHelper.FixQuery(commands[i], query));
            if (p == Priority.Invisible)
            {
                commands.RemoveAt(i);
                i--;
            }
            else
                priorities.Add((int)p);
        }
        
        List<ICommand> clone = new List<ICommand>();
        clone.AddRange(commands);

        Comparison<ICommand> comparison = ((ICommand c1, ICommand c2) => { return -priorities[clone.IndexOf(c1)].CompareTo(priorities[clone.IndexOf(c2)]); });
        commands.Sort(comparison);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickNav.Helper;

internal class TaskRegistryHelper
{
    private static List<Task> RunningTasks = new List<Task>();

    public static void Add(Task task)
    {
        RunningTasks.Add(task);
    }

    public static void EndAllTasks()
    {
        foreach(var task in RunningTasks)
        {
            task.Dispose();
        }
    }
}

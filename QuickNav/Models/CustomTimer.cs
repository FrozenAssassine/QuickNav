using System;
using System.Diagnostics;
using System.Timers;

namespace QuickNav.Models;

internal class CustomTimer
{
    private Timer t;
    private int timeInSeconds;
    private Action onTimeReached;
    private Action<int> timerUpdated;
    DateTime startTime;

    public void SetTimer(int timeInSeconds, Action onTimeReachedCallback, Action<int> timerUpdated)
    {
        this.timeInSeconds = timeInSeconds;
        this.onTimeReached = onTimeReachedCallback;
        this.timerUpdated = timerUpdated;

        t = new Timer();
        t.Interval = 1000;
        t.Start();
        t.Elapsed += T_Elapsed;

        timerUpdated?.Invoke(timeInSeconds);
        startTime = DateTime.Now;
    }

    private void T_Elapsed(object sender, ElapsedEventArgs e)
    {
        TimeSpan elapsedTime = DateTime.Now - startTime;
        int remainingSeconds = timeInSeconds - (int)elapsedTime.TotalSeconds;

        // Invoke timerUpdated event with remainingSeconds
        timerUpdated?.Invoke(remainingSeconds);

        if (remainingSeconds <= 0)
        {
            t.Stop();
            onTimeReached?.Invoke();
        }
    }
}
using QuickNav.Helper;
using QuickNav.Models;
using System;
using System.Diagnostics;

namespace QuickNav.Widgets;

public sealed partial class TimerWidget : QuickNavWidget
{
    private CustomTimer timer = new CustomTimer();

    public TimerWidget(int timeInSeconds)
    {
        this.InitializeComponent();
        this.Activate();
        Title = "QuickNav Timer";
        
        timer.SetTimer(timeInSeconds, TimerElapsed,(seconds) => UpdateUI(seconds));
        this.SetTitleBar(titlebar);
        this.IsShownInSwitchers = true;
        this.IsAlwaysOnTop = true;
    }

    public void UpdateUI(int seconds)
    {
        var ts = new TimeSpan(0, 0, seconds);
        this.DispatcherQueue.TryEnqueue(() =>
        {
            timeDisplay.Text = $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
        });
    }

    public void TimerElapsed()
    {

        //TODO play sound or so
        Debug.WriteLine("YOUR TIMER IS OVER YOU DUMBASS");
    }


    private void ToggleTopMost_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        this.IsAlwaysOnTop = !this.IsAlwaysOnTop;
    }

    private void CloseTimer_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        timer.EndTimer();
        TimerHostHelper.EndTimer(this);
        this.Close();
    }
}

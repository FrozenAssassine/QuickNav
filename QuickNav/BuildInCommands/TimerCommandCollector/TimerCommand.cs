using QuickNav.Helper;
using QuickNavPlugin;
using QuickNavPlugin.UI;
using System;
using System.Text.RegularExpressions;
using Windows.Services.Maps.LocalSearch;

namespace QuickNav.BuildInCommands.TimerCommandCollector
{
    internal class TimerCommand : ICommand
    {
        public string Description => "Create timer (timer:5h 2min 10sec), (timer: 123sec), (timer:5hours 40minutes 10s)";

        public Uri Icon(string query) => new Uri("ms-appx://App/Assets/commands/timer.png");

        public Priority Priority(string query)
        {
            return QuickNavPlugin.Priority.Low;
        }

        public string CommandTrigger => "timer:";

        public string[] Keywords => new string[] { "timer"};

        private static (int hours, int minutes, int seconds) ParseTime(string input)
        {
            // Define patterns for different time units
            string timePattern = @"(\d+)\s*(seconds|second|secs|sec|s|minutes|minute|mins|min|m|hours|hour|hrs|hr|h)";
            MatchCollection matches = Regex.Matches(input, timePattern, RegexOptions.IgnoreCase);

            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            foreach (Match match in matches)
            {
                int value = int.Parse(match.Groups[1].Value);
                string unit = match.Groups[2].Value.ToLower();

                switch (unit)
                {
                    case "hour":
                    case "hours":
                    case "hr":
                    case "hrs":
                    case "h":
                        hours += value;
                        break;
                    case "minute":
                    case "minutes":
                    case "min":
                    case "mins":
                    case "m":
                        minutes += value;
                        break;
                    case "second":
                    case "seconds":
                    case "sec":
                    case "secs":
                    case "s":
                        seconds += value;
                        break;
                    default:
                        break;
                }
            }
            return (hours, minutes, seconds);
        }

        public string Name(string query)
        {
            if (query.Length == 0)
                return "Create a timer";
            var time = ParseTime(query);
            return $"Timer for " + (time.hours == 0 ? "" : time.hours + "hours ") + (time.minutes == 0 ? "" : time.minutes+ "minutes ") + (time.seconds == 0 ? "" : time.seconds + "seconds");
        }

        public bool RunCommand(string query, out ContentElement content)
        {
            content = null;

            var time = ParseTime(query);
            if (query.Length == 0)
                return false;

            TimerHostHelper.HostTimer(time);

            return true;
        }

        public void OnWindowOpened() { }
    }
}

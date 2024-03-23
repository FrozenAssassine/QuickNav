using raminrahimzada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.BuildInCommands
{
    public static class CommandSettings
    {
        // Calculator command
        public static bool InsertLostTimesSymbols = true;
        public static bool Radians = true;
        public static int MaxTaylorIterations { get => DecimalMath.MaxIteration; set { DecimalMath.MaxIteration = value; } }

        // File search command
        public static int AmountOfFiles = 30;
    }
}

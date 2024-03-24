using QuickNav.Core;
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

        // Save
        public static void SaveAll()
        {
            // Calculator command
            AppSettings.SaveSettings("InsertLostTimesSymbols", InsertLostTimesSymbols);
            AppSettings.SaveSettings("Radians", Radians);
            AppSettings.SaveSettings("MaxTaylorIterations", MaxTaylorIterations);

            // File search command
            AppSettings.SaveSettings("AmountOfFiles", AmountOfFiles);
        }
        
        // Load
        public static void LoadAll()
        {
            // Calculator command
            InsertLostTimesSymbols = AppSettings.GetSettingsAsBool("InsertLostTimesSymbols", InsertLostTimesSymbols);
            Radians = AppSettings.GetSettingsAsBool("Radians", Radians);
            MaxTaylorIterations = AppSettings.GetSettingsAsInt("MaxTaylorIterations", MaxTaylorIterations);

            // File search command
            AmountOfFiles = AppSettings.GetSettingsAsInt("AmountOfFiles", AmountOfFiles);
        }
    }
}

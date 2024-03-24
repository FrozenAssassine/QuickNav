using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.Helper
{
    public class StartupHelper
    {
        public static bool AddToStartup(string appName/*, string appPath*/)
        {
            throw new NotImplementedException();
            /*try
            {
                string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                string shortcutPath = Path.Combine(startupFolderPath, appName + ".lnk");

                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);

                shortcut.TargetPath = appPath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(appPath);
                shortcut.Save();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }*/
        }

        public static bool StartupExists(string appName)
        {
            string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            string shortcutPath = Path.Combine(startupFolderPath, appName + ".lnk");

            return File.Exists(shortcutPath);
        }
        
        public static bool RemoveFromStartup(string appName)
        {
            string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            string shortcutPath = Path.Combine(startupFolderPath, appName + ".lnk");

            if (File.Exists(shortcutPath))
            {
                File.Delete(shortcutPath);
                return true;
            }

            return false;
        }
    }
}

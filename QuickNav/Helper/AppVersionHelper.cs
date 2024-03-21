using Windows.ApplicationModel;

namespace QuickNav.Helper
{
    internal class AppVersionHelper
    {
        public static string GetAppVersion()
        {
            return Package.Current.Id.Version.Major + "." +
                Package.Current.Id.Version.Minor + "." +
                Package.Current.Id.Version.Build;
        }
    }
}

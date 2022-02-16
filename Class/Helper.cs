using System.Text;
using System.Windows;

namespace RunAsManager
{
    internal static class Helper
    {
        public static byte[] GetBytes(string text, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            return encoding.GetBytes(text);
        }

        public static string GetStringFromBytes(byte[] text, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            return encoding.GetString(text);
        }

        public static Window GetWindow(object dataContext)
        {
            if (Application.Current == null)
                return null;

            foreach (Window window in Application.Current.Windows)
                if (window.DataContext == dataContext)
                    return window;

            return null;
        }
    }
}

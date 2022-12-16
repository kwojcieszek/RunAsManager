using System.Diagnostics;
using System.Security;

namespace RunAsManager
{
    internal class RunAs
    {
        public Process? Start(string userName, SecureString password, string path, string args = "")
        {
            var process = new ProcessStartInfo(path)
            {
                LoadUserProfile = true,
                UseShellExecute = false,
                UserName = userName,
                Password = password,
                WorkingDirectory = System.IO.Directory.GetDirectoryRoot(path),
                Arguments = args
            };

            return Process.Start(process);
        }
    }
}
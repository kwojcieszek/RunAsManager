using IWshRuntimeLibrary;
using System;
using System.IO;

namespace RunAsManager
{
    public class WindowsLink
    {
        public void Create(string name, string arguments, string description, string path, string targetPath, string workingDirectory, string iconLocation)
        {
            var wsh = new WshShell();

            if (wsh.CreateShortcut($"{path}\\{name}.lnk") is not IWshShortcut shortcut)
                throw new Exception("Error create link.");

            shortcut.Arguments = arguments;
            shortcut.TargetPath = targetPath;
            shortcut.WindowStyle = 1;
            shortcut.Description = description;
            shortcut.WorkingDirectory = workingDirectory;
            shortcut.IconLocation = iconLocation;
            shortcut.Save();
        }

        public (string, string) GetShortcutTargetFile(string shortcutFilename)
        {
            var wsh = new WshShell();
            var shortcut = wsh.CreateShortcut(shortcutFilename) as IWshRuntimeLibrary.IWshShortcut;

            return (shortcut.TargetPath, Path.GetFileNameWithoutExtension(shortcut.FullName));
        }
    }
}
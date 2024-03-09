using Microsoft.Win32;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
internal static class WinRARHelper {
    public static string? GetWinRARInstallationPath() {
        string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
        using RegistryKey? key = Registry.LocalMachine.OpenSubKey(registryKey);
        return key?.GetValue("Path")?.ToString();
    }
}

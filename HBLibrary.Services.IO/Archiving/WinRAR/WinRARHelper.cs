using Microsoft.Win32;
using System.Diagnostics;

namespace HBLibrary.Services.IO.Archiving.WinRAR;
internal static class WinRARHelper {
    public static string? GetWinRARPath() {
        return GetWinRARInstallationPath() ?? GetPathVariable();
    }

    public static string? GetWinRARInstallationPath() {
        string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
        using RegistryKey? key = Registry.LocalMachine.OpenSubKey(registryKey);
        return key?.GetValue("Path")?.ToString();
    }

    public static string? GetPathVariable() {
        string? pathVariable = Environment.GetEnvironmentVariable("PATH");
        if (pathVariable is null)
            return null;

        foreach (var path in pathVariable.Split(';')) {
            var rarPath = Path.Combine(path, "Rar.exe");
            if (File.Exists(rarPath))
                return rarPath;

            var unrarPath = Path.Combine(path, "UnRAR.exe");
            if (File.Exists(unrarPath)) 
                return unrarPath;
        }

        return null;
    }
}

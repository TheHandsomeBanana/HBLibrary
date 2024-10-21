using HBLibrary.Interface.IO;
using Microsoft.Win32;

namespace HBLibrary.IO.Archiving.WinRAR;
public static class WinRARManager {
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

    public static bool CheckWinRARInstallationForPath(string path) {
        if (!PathValidator.ValidatePath(path) || !Directory.Exists(path))
            return false;

        foreach (string filename in Directory.EnumerateFiles(path)) {
            if (filename.Split('\\').Last() == "Rar.exe")
                return true;
        }

        return false;
    }

    private static readonly string[] licenseKeyPaths = [
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinRAR"),
        @"C:\Program Files\WinRAR",
        @"C:\Program Files (x86)\WinRAR"
    ];

    public static bool CheckWinRARLicense(out string licenseKeyPath) {
        licenseKeyPath = "";

        foreach (string licenseKey in licenseKeyPaths.Where(Directory.Exists)) {
            if (Directory.EnumerateFiles(licenseKey).Any(e => e.Split('\\').Last() == "rarreg.key")) {
                licenseKeyPath = Path.Combine(licenseKey, "rarreg.key");
                return ValidateLicenseFile(Path.Combine(licenseKey, "rarreg.key"));
            }
        }

        return false;
    }

    public static bool ValidateLicenseFile(string filename) {
        try {
            string[] content = File.ReadAllLines(filename);
            if (content.Length < 11)
                return false;

            if (content[0] != "RAR registration data")
                return false;

            if (string.IsNullOrWhiteSpace(content[1]))
                return false;

            if (string.IsNullOrWhiteSpace(content[2]))
                return false;

            if (!content[3].StartsWith("UID="))
                return false;

            string uid = content[3].Substring(4);

            if (uid.Length != 20)
                return false;

            for (int i = 4; i < 11; i++) {
                if (content[i].Length != 54 || content[i].Any(e => !Uri.IsHexDigit(e)))
                    return false;
            }
        }
        catch {
            return false;
        }

        return true;
    }
}

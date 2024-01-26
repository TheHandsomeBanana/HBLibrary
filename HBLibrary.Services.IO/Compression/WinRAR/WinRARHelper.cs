using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.WinRAR {
    internal static class WinRARHelper {
        public static string? GetWinRARInstallationPath() {
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
            using RegistryKey? key = Registry.LocalMachine.OpenSubKey(registryKey);
            if (key == null)
                return null;

            return key.GetValue("Path")?.ToString();
        }
    }


}

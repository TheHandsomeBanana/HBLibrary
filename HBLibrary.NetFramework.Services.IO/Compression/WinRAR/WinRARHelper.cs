using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.IO.Compression.WinRAR {
    internal static class WinRARHelper {
        public static string GetWinRARInstallationPath() {
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey)) {
                if (key != null) {
                    object value = key.GetValue("Path");
                    if (value != null) 
                        return value.ToString();
                }
            }

            return null; // WinRAR not found in registry
        }
    }


}

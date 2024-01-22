using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.IO.Compression.WinRAR {
    public class WinRARCompressor : IWinRARCompressor {
        private readonly string winRARPath;
        public WinRARCompressor() {
            this.winRARPath = WinRARHelper.GetWinRARInstallationPath()
                ?? throw new DirectoryNotFoundException("Could not detect the WinRAR installation directory.");
        }

        public void CompressDirectory(string sourceDirectory, string destinationArchive) {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = winRARPath + "\\Rar.exe"; // Path to Rar.exe
            startInfo.Arguments = $"a -r \"{destinationArchive}\" \"{sourceDirectory}\\*\""; // 'a' for adding to archive, '-r' for recursive
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (Process exeProcess = Process.Start(startInfo)) {
                exeProcess.WaitForExit();
            }
        }

        public void CompressDirectory(string sourceDirectory, WinRARCompressionSettings settings) {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = winRARPath + "\\Rar.exe"; // Path to Rar.exe
            // TODO specify arguments
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (Process exeProcess = Process.Start(startInfo)) {
                exeProcess.WaitForExit();
            }
        }

        public void CompressFile(string sourceFile, string destinationArchive) {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = winRARPath + "\\Rar.exe";
            startInfo.Arguments = $"a -r \"{destinationArchive}\" \"{sourceFile}\"";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            using(Process exeProcess = Process.Start(startInfo))
                exeProcess.WaitForExit();
        }

        public void CompressFile(string sourceFile, WinRARCompressionSettings settings) {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = winRARPath + "\\Rar.exe"; // Path to Rar.exe
            // TODO specify arguments
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (Process exeProcess = Process.Start(startInfo)) {
                exeProcess.WaitForExit();
            }
        }

        public void Extract(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings) {
            throw new NotImplementedException();
        }

        public void Extract(string sourceArchive, string destinationDirectory) {
            Extract(sourceArchive, destinationDirectory, null);
        }

        public void ExtractDirectory(string sourceArchive, string destinationPath) {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = winRARPath + "\\UnRAR.exe"; // Path to UnRAR.exe
            startInfo.Arguments = $"x \"{sourceArchive}\" \"{destinationPath}\\\""; // 'x' for extract
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (Process exeProcess = Process.Start(startInfo))
                exeProcess.WaitForExit();
        }

        public void ExtractFile(string sourceArchive, string destinationPath) {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = winRARPath + "\\UnRAR.exe"; // Path to UnRAR.exe
            startInfo.Arguments = $"x \"{sourceArchive}\" \"{destinationPath}\""; // 'x' for extract
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (Process exeProcess = Process.Start(startInfo))
                exeProcess.WaitForExit();
        }
    }
}

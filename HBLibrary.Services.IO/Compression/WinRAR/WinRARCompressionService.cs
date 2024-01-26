using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.WinRAR {
    public class WinRARCompressionService : IWinRARCompressionService {
        private readonly string winRARPath;
        public WinRARCompressionService() {
            this.winRARPath = WinRARHelper.GetWinRARInstallationPath()
                ?? throw new DirectoryNotFoundException("Could not detect the WinRAR installation directory.");
        }

        public void Compress(string sourceFile, string destinationArchive, WinRARCompressionSettings settings) {
            // TODO specify arguments
            //$"a -r \"{destinationArchive}\" \"{sourceDirectory}\\*\""; // 'a' for adding to archive, '-r' for recursive

            string arguments = settings.ToString()!;
            using Process? exeProcess = Process.Start(GetProcessStartInfo(arguments));
            exeProcess?.WaitForExit();
        }

        public void Extract(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings) {
            //$"x \"{sourceArchive}\" \"{destinationDirectory}\""; // 'x' for extract

            string arguments = settings.ToString()!;
            using Process? exeProcess = Process.Start(GetProcessStartInfo(arguments));
            exeProcess?.WaitForExit();
        }

#if NET8_0_OR_GREATER
        public Task? CompressAsync(string sourceFile, string destinationArchive, WinRARCompressionSettings settings) {
            string arguments = settings.ToString()!;
            using Process? exeProcess = Process.Start(GetProcessStartInfo(arguments));
            return exeProcess?.WaitForExitAsync();
        }

        public Task? ExtractAsync(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings) {
            string arguments = settings.ToString()!;
            using Process? exeProcess = Process.Start(GetProcessStartInfo(arguments));
            return exeProcess?.WaitForExitAsync();
        }
#endif

        public void CompressDirectory(string sourceDirectory, string destinationArchive)
           => Compress(sourceDirectory, destinationArchive, new WinRARCompressionSettings());


        public void CompressFile(string sourceFile, string destinationArchive)
           => Compress(sourceFile, destinationArchive, new WinRARCompressionSettings());


        public void Extract(string sourceArchive, string destinationDirectory)
           => Extract(sourceArchive, destinationDirectory, new WinRARExtractionSettings());
        

        private ProcessStartInfo GetProcessStartInfo(string arguments) {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = winRARPath + "\\WinRAR.exe";
            startInfo.Arguments = arguments;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            return startInfo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.IO.Compression.WinRAR {
    public interface IWinRARCompressor : ICompressor {
        void CompressFile(string sourceFile, WinRARCompressionSettings settings);
        void CompressDirectory(string sourceDirectory, WinRARCompressionSettings settings);
        void Extract(string sourceArchive, string destinationDirectory, WinRARExtractionSettings settings);
    }
}

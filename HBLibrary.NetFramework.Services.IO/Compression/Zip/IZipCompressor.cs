using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.IO.Compression.Zip {
    public interface IZipCompressor : ICompressor {
        void CompressFile(string sourceFile, string destinationArchive, ZipCompressionSettings settings);
        void CompressDirectory(string sourceDirectory, string destinationDirectory, ZipCompressionSettings settings);
        void Extract(string sourceArchive, string destinationDirectory, ZipExtractionSettings settings);
    }
}

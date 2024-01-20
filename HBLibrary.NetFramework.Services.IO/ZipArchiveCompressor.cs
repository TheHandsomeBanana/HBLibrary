using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.IO {
    public class ZipArchiveCompressor : IFileCompressor {
        public void CompressFile(string fileName, string compressedFileName) {
            string entryName = Path.GetFileNameWithoutExtension(compressedFileName) + Path.GetExtension(fileName);

            using (FileStream originalFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (FileStream compressStream = new FileStream(compressedFileName, FileMode.CreateNew))
            using (var archive = new ZipArchive(compressStream, ZipArchiveMode.Create)) {
                var zipArchiveEntry = archive.CreateEntry(entryName);
                using (var destination = zipArchiveEntry.Open()) {
                    originalFileStream.CopyTo(destination);
                }
            }
        }
    }
}

using HBLibrary.Services.IO.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime;
using HBLibrary.Services.IO.Compression;

namespace HBLibrary.Services.IO.Archiving.Zip {
    public class ZipCompressor : IZipCompressor {

        public void Compress(Archive archive) {
            Compress(archive, new ZipCompressionSettings());
        }

        public void Compress(Archive archive, ZipCompressionSettings settings) {
            using (ZipFile zip = new ZipFile()) {
                if (settings.Password != null) {
                    zip.Password = settings.Password;
                    zip.Encryption = settings.EncryptionAlgorithm;
                }

                zip.CompressionLevel = settings.Level;
                zip.CompressionMethod = settings.Method;

                foreach (string dir in archive.GetDirectoryNames())
                    zip.AddDirectory(dir, Path.GetDirectoryName(dir));

                zip.AddFiles(archive.GetFileNames());
                zip.Save(archive.Name);
            }
        }
    }
}

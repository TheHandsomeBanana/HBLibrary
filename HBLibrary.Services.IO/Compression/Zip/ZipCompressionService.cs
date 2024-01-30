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

namespace HBLibrary.Services.IO.Compression.Zip {
    public class ZipCompressionService : IZipCompressionService {

        public void CompressDirectory(string sourceDirectory, string destinationArchive) {
            CompressDirectory(sourceDirectory, destinationArchive, new ZipCompressionSettings());
        }

        public void Extract(string sourceArchive, string destinationDirectory) {
            Extract(sourceArchive, destinationDirectory, new ZipExtractionSettings());
        }

        public void Compress(string sourceFile, string destinationArchive) {
            CompressFile(sourceFile, destinationArchive, new ZipCompressionSettings()); 
        }

        public void CompressFile(string sourceFile, string destinationArchive, ZipCompressionSettings settings) {
            using(ZipFile zip = new ZipFile()) {
                if(settings.Password != null) {
                    zip.Password = settings.Password;
                    zip.Encryption = settings.EncryptionAlgorithm;
                }

                zip.CompressionLevel = settings.Level;
                zip.CompressionMethod = settings.Method;
                zip.AddFile(sourceFile);
                zip.Save(destinationArchive);
            }
        }

        public void CompressDirectory(string sourceDirectory, string destinationArchive, ZipCompressionSettings settings) {
            using (ZipFile zip = new ZipFile()) {
                if (settings.Password != null) {
                    zip.Password = settings.Password;
                    zip.Encryption = settings.EncryptionAlgorithm;
                }

                zip.CompressionLevel = settings.Level;
                zip.CompressionMethod = settings.Method;

                zip.AddDirectory(sourceDirectory, Path.GetDirectoryName(sourceDirectory));
                zip.Save(destinationArchive);
            }
        }

        public void Extract(string sourceArchive, string destinationDirectory, ZipExtractionSettings settings) {
            using (ZipFile zip = ZipFile.Read(sourceArchive)) {
                if (settings.Password != null)
                    zip.Password = settings.Password;

                zip.ExtractAll(destinationDirectory, settings.ExtractExistingFileAction);
            }
        }
    }
}

using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.Zip {
    public class ZipCompressionSettings {
        public string? Password { get; set; }
        public EncryptionAlgorithm EncryptionAlgorithm { get; set; } = EncryptionAlgorithm.WinZipAes256;
        public CompressionMethod Method { get; set; } = CompressionMethod.Deflate;
        public CompressionLevel Level { get; set; } = CompressionLevel.BestCompression;

        public static ZipCompressionSettings Default => new ZipCompressionSettings();
    }
}

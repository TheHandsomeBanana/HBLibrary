using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.Zip {
    public class ZipCompressionSettings {
        public string Password { get; set; }
        public EncryptionAlgorithm EncryptionAlgorithm { get; set; } = EncryptionAlgorithm.WinZipAes256;
    }
}

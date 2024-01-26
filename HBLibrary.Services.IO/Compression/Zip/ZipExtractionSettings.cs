using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Compression.Zip {
    public class ZipExtractionSettings {
        public string? Password { get; set; }
        public ExtractExistingFileAction ExtractExistingFileAction { get; set; } = ExtractExistingFileAction.OverwriteSilently;
    }
}

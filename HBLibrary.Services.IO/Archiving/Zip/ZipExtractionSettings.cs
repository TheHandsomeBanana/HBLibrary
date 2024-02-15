using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Archiving.Zip {
    public class ZipExtractionSettings {
        public string? Password { get; set; }
        public ExtractExistingFileAction ExtractExistingFileAction { get; set; } = ExtractExistingFileAction.OverwriteSilently;

        public static ZipExtractionSettings Default => new ZipExtractionSettings();
    }
}

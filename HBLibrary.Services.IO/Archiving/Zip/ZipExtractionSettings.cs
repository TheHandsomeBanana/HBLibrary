using Ionic.Zip;

namespace HBLibrary.Services.IO.Archiving.Zip {
    public class ZipExtractionSettings {
        public string? Password { get; set; }
        public ExtractExistingFileAction ExtractExistingFileAction { get; set; } = ExtractExistingFileAction.OverwriteSilently;

        public static ZipExtractionSettings Default => new ZipExtractionSettings();
    }
}

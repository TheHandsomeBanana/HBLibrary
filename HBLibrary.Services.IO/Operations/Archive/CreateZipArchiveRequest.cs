using HBLibrary.Services.IO.Archiving.Zip;

namespace HBLibrary.Services.IO.Operations.Archive;
public class CreateZipArchiveRequest : CreateArchiveRequest {
    public override bool CanAsync => false;
    public required IZipCompressor Compressor { get; set; }
    public ZipCompressionSettings CompressionSettings { get; set; } = ZipCompressionSettings.Default;
}

using HBLibrary.Services.IO.Archiving.WinRAR;
using HBLibrary.Services.IO.Archiving.WinRAR.Obsolete.Options;

namespace HBLibrary.Services.IO.Operations.Archive;
public class CreateWinRARArchiveRequest : CreateArchiveRequest {
    public override bool CanAsync =>
#if NET5_0_OR_GREATER
        true;
#elif NET472_OR_GREATER
        false;
#endif

    public required IWinRARCompressor Compressor { get; set; }
    public WinRARCompressionOptions CompressionSettings { get; set; } = WinRARCompressionOptions.Default;
}

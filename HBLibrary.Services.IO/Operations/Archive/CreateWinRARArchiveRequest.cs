using HBLibrary.Services.IO.Archiving.WinRAR;
using HBLibrary.Services.IO.Archiving.WinRAR.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

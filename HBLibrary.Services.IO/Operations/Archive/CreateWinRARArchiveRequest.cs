using HBLibrary.Services.IO.Compression.WinRAR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.Archive;
public class CreateWinRARArchiveRequest : IOOperationRequest {
    public override bool CanAsync =>
#if NET5_0_OR_GREATER
        true;
#elif NET472_OR_GREATER
        false;
#endif

    public required IWinRARCompressor Compressor { get; set; }
}

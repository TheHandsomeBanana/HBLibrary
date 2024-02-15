using HBLibrary.Services.IO.Archiving.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.Archive;
public class CreateZipArchiveRequest : CreateArchiveRequest {
    public override bool CanAsync => false;
    public required IZipCompressor Compressor { get; set; }
    public ZipCompressionSettings CompressionSettings { get; set; } = ZipCompressionSettings.Default;
}

using HBLibrary.Services.IO.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.Archive;
public class CreateArchiveRequest : IOOperationRequest {
    public override bool CanAsync => false;
    public required ICompressor Compressor { get; set; }
    public required IArchive Archive { get; set; }
}

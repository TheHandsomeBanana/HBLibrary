using HBLibrary.Services.IO.Archiving;
using HBLibrary.Services.IO.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.Archive;
public abstract class CreateArchiveRequest : IOOperationRequest {
    public override bool CanAsync => false;
    public required Archiving.Archive Archive { get; set; }
}

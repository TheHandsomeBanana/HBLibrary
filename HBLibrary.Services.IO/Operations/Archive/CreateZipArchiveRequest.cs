using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.Archive;
public class CreateZipArchiveRequest : IOOperationRequest {
    public override bool CanAsync => false;
}

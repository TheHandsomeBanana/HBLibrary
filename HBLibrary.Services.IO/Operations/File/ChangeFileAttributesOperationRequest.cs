using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.File;
internal class ChangeFileAttributesOperationRequest : FileOperationRequest
{
    public override bool CanAsync => false;
}

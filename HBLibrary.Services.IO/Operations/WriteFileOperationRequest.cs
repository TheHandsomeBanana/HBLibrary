using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations;
public class WriteFileOperationRequest : FileOperationRequest {
    public override bool CanAsync => true;
}

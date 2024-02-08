using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations;
public abstract class IOOperationRequest {
    public ValidPath ValidPath { get; set; }
    public abstract bool CanAsync { get; }
}

using HBLibrary.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations;
public abstract class FileOperationRequest {
    public FileSnapshot File { get; init; }
    public FileContentType ContentType { get; init; }
    public abstract bool CanAsync { get; }
    

    public override string ToString() {
        return base.ToString();
    }
}
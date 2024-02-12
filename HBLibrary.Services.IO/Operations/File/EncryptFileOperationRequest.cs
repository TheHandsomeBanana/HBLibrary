using HBLibrary.Services.Security.Cryptography;
using HBLibrary.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.File;
public class EncryptFileOperationRequest : WriteFileOperationRequest {
    public required ICryptographer Cryptographer { get; set; }
    public required CryptographySettings Settings { get; set; }
    public override bool Append { get => false; set => base.Append = false; }
}

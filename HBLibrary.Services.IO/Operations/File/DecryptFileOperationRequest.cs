using HBLibrary.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.File;
public class DecryptFileOperationRequest : ReadFileOperationRequest {
    public required CryptographySettings Settings { get; set; }
}

using HBLibrary.Services.Security.Cryptography;
using HBLibrary.Services.Security.Cryptography.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Operations.File;
public class DecryptFileRequest : ReadFileRequest {
    public required ICryptographer Cryptographer { get; set; }
    public required CryptographySettings Settings { get; set; }
}

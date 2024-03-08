using HBLibrary.Services.Security.Cryptography;
using HBLibrary.Services.Security.Cryptography.Settings;

namespace HBLibrary.Services.IO.Operations.File;
public class DecryptFileRequest : ReadFileRequest {
    public required ICryptographer Cryptographer { get; set; }
    public required CryptographySettings Settings { get; set; }
}

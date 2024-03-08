using HBLibrary.Services.Security.Cryptography;
using HBLibrary.Services.Security.Cryptography.Settings;

namespace HBLibrary.Services.IO.Operations.File;
public class EncryptFileRequest : WriteFileRequest {
    public required ICryptographer Cryptographer { get; set; }
    public required CryptographySettings Settings { get; set; }
    public override bool Append { get => false; set => base.Append = false; }
}

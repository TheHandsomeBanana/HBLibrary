using HBLibrary.Services.Security.Cryptography.Keys;

namespace HBLibrary.Services.Security.Cryptography.Settings;
public class CryptographySettings {
    public required CryptographyMode Mode { get; init; }
    public required IKey Key { get; init; }


}

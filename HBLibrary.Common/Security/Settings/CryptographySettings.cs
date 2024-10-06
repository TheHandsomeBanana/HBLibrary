using HBLibrary.Common.Security.Keys;

namespace HBLibrary.Common.Security.Settings;
public class CryptographySettings {
    public required CryptographyMode Mode { get; init; }
    public required IKey Key { get; init; }


}

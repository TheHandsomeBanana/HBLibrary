using HBLibrary.Interface.Security.Keys;
using System.Text;

namespace HBLibrary.Interface.Security;
public class CryptographyInput {
    public required CryptographyMode Mode { get; init; }
    public required IKey Key { get; init; }
}

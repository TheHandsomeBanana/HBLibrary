using HBLibrary.Common.Security.Keys;
using System.Text;

namespace HBLibrary.Common.Security;
public class CryptographyInput
{
    public required CryptographyMode Mode { get; init; }
    public required IKey Key { get; init; }
}

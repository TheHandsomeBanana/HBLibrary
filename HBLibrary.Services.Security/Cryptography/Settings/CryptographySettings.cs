using HBLibrary.Services.Security.Cryptography.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Security.Cryptography.Settings;
public class CryptographySettings {
    public CryptographyMode Mode { get; }
    public required IKey Key { get; init; }

    
}

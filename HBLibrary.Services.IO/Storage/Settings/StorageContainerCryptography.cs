using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Settings;
public class StorageContainerCryptography {
    public required Func<IKey> GetEntryKey { get; set; }
    public required CryptographyMode CryptographyMode { get; set; }
}

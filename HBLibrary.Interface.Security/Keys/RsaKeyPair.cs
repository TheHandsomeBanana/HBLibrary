using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Security.Keys;
[Serializable]
public class RsaKeyPair : IKey
{
    public required RsaKey PublicKey { get; set; }
    public required RsaKey PrivateKey { get; set; }

    [JsonIgnore]
    public string Name => nameof(RsaKeyPair);
}

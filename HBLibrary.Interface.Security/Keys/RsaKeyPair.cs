using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HBLibrary.Interface.Security.Keys;
[Serializable]
public class RsaKeyPair : IKey {
    [JsonIgnore]
    [XmlIgnore]
    public bool IsDisposed { get; private set; }

    public required RsaKey? PublicKey { get; set; }
    public required RsaKey? PrivateKey { get; set; }

    [JsonIgnore]
    public string Name => nameof(RsaKeyPair);


    public void Dispose() {
        PublicKey?.Dispose();
        PrivateKey?.Dispose();

        PublicKey = null;
        PrivateKey = null;
    }
}

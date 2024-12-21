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

    private RsaKey? publicKey;
    public RsaKey? PublicKey { 
        get {
            if(IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKeyPair));
            }

            return publicKey;
        } 
        set {
            if (IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKeyPair));
            }

            publicKey = value; 
        }
    }
    
    private RsaKey? privateKey;
    public RsaKey? PrivateKey { 
        get {
            if(IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKeyPair));
            }

            return privateKey;
        } 
        set {
            if (IsDisposed) {
                throw new ObjectDisposedException(nameof(RsaKeyPair));
            }

            privateKey = value; 
        }
    }

    [JsonIgnore]
    public string Name => nameof(RsaKeyPair);


    public void Dispose() {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(RsaKey));
        }

        PublicKey?.Dispose();
        PrivateKey?.Dispose();

        PublicKey = null;
        PrivateKey = null;

        IsDisposed = true;
    }
}

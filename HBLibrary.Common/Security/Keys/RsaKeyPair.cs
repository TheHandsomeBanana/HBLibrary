using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Security.Keys;
public class RsaKeyPair {
    public required RsaKey PublicKey { get; set; }
    public required RsaKey PrivateKey { get; set; }
}

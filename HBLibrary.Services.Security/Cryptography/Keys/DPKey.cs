using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.Security.Cryptography.Keys;
public class DPKey : IKey {
    public string Name => nameof(DPKey);
    public byte[] Key { get; set; } = [];

    public DPKey(byte[] key) {
        Key = key;
    }
}

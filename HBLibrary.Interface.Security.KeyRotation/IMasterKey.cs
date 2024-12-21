using HBLibrary.Core;
using HBLibrary.Interface.Security.Keys;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Security.KeyRotation;
public interface IMasterKey : IDisposable {
    public SecureString Salt { get; }
    public SecureString Value { get; }
    public string ReadValue();
    public AesKey ConvertToAesKey();
}

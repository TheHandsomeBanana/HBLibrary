using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Security.KeyRotation;
public interface IKeyRotator {
    public Task RotateKeysAsync(IMasterKey masterKey, string keyFile);
}

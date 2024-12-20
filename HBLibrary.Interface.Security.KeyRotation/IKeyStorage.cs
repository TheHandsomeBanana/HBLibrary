using HBLibrary.Core.Extensions;
using HBLibrary.DataStructures;
using HBLibrary.Interface.Security.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Security.KeyRotation;
public interface IKeyStorage {
    public Task<Result> AddFilesToKeyMap(string keyId, IKey key, string[] files);
    public Task<Result> SaveKeyAsync(string keyId, IKey key);
    public Task<Result<IKey>> GetKeyAsync(string keyId);
}

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
    /// <summary>
    /// Adds <paramref name="files"/> to the key file and encrypts them using the existing <see cref="IKey"/>.
    /// </summary>
    /// <param name="keyId"></param>
    /// <param name="files"></param>
    /// <returns></returns>
    public Task<Result> AddFilesAsync(string keyId, string[] files);
    /// <summary>
    /// If the <paramref name="keyId"/> exists, it will replace the <paramref name="key"/>, decrypt and reencrypt existing files.
    /// <br>If the <paramref name="keyId"/> does not exist, it will create a new entry with the provided <paramref name="key"/></br>
    /// </summary>
    /// <param name="keyId"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<Result> SaveKeyAsync(string keyId, IKey key);
    /// <summary>
    /// </summary>
    /// <param name="keyId"></param>
    /// <returns>The possibly decrypted <see cref="IKey"/> for the provided <paramref name="keyId"/></returns>
    public Task<Result<IKey>> GetKeyAsync(string keyId);
}

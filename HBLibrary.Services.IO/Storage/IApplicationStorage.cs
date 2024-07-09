using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Config;
using HBLibrary.Services.IO.Storage.Container;
using HBLibrary.Services.IO.Storage.Entries;
using HBLibrary.Services.IO.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage;
public interface IApplicationStorage {
    public string BasePath { get; }
    public Guid DefaultContainerId { get; }
    public IStorageEntry? GetStorageEntry(Guid containerId, string filename);
    public IStorageEntry? CreateStorageEntry(Guid containerId, string filename, StorageEntryContentType contentType);
    public void AddOrUpdateStorageEntry(Guid containerId, string filename, StorageEntryContentType contentType);
    public bool ContainsEntry(Guid containerId, string filename);

    public IEnumerable<IStorageEntry> GetStorageEntries(Guid containerId);
    public void SaveStorageEntries(Guid containerId);
    public void SaveAll();
    public IStorageEntryContainer GetContainer(Guid containerId);

}

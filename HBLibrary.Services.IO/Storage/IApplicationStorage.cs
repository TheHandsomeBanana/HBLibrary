using HBLibrary.Services.IO.Storage.Builder;
using HBLibrary.Services.IO.Storage.Container;
using HBLibrary.Services.IO.Storage.Entries;

namespace HBLibrary.Services.IO.Storage;
public interface IApplicationStorage {
    public string BasePath { get; }
    public Guid DefaultContainerId { get; }
    public IStorageEntry? GetStorageEntry(Guid containerId, string filename);
    public IStorageEntry? CreateStorageEntry(Guid containerId, string filename, StorageEntryContentType contentType);
    public void AddOrUpdateStorageEntry(Guid containerId, string filename, object entry, StorageEntryContentType contentType);
    public bool ContainsEntry(Guid containerId, string filename);

    public IEnumerable<IStorageEntry> GetStorageEntries(Guid containerId);
    public void SaveStorageEntries(Guid containerId);
    public void SaveAll();

    public IStorageEntryContainer GetContainer(Guid containerId);
    public void CreateContainer(Guid containerId, Func<IStorageEntryContainerBuilder, IStorageEntryContainer> builder);
    public bool RemoveContainer(Guid containerId);
    public void RemoveAllContainers();
}

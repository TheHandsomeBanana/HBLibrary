using HBLibrary.Services.IO.Storage.Builder;
using HBLibrary.Services.IO.Storage.Container;
using HBLibrary.Services.IO.Storage.Entries;

namespace HBLibrary.Services.IO.Storage;
public interface IApplicationStorage {
    public string BasePath { get; }
    public Guid DefaultContainerId { get; }
    public IStorageEntryContainer DefaultContainer { get; }

    public IEnumerable<IStorageEntry> GetStorageEntries(Guid containerId);
    public void SaveStorageEntries(Guid containerId);
    public void SaveAll();


    public IStorageEntryContainer GetContainer(Guid containerId);
    public IStorageEntryContainer GetContainer(Type containerType);
    public void CreateContainer(Guid containerId, Func<IStorageEntryContainerBuilder, IStorageEntryContainer> builder);
    public bool RemoveContainer(Guid containerId);
    public void RemoveAllContainers();

    public Task SaveStorageEntriesAsync(Guid containerId);
    public Task SaveAllAsync();
}

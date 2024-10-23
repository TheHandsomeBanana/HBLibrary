using HBLibrary.Interface.IO.Storage.Builder;
using HBLibrary.Interface.IO.Storage.Container;
using HBLibrary.Interface.IO.Storage.Entries;

namespace HBLibrary.Interface.IO.Storage;
public interface IApplicationStorage : IDisposable {
    public string BasePath { get; }
    public Guid DefaultContainerId { get; }
    public IStorageEntryContainer DefaultContainer { get; }
    public bool? HasUnsavedChanges();

    public IEnumerable<IStorageEntry> GetStorageEntries(Guid containerId);
    public void SaveStorageEntries(Guid containerId);
    public void SaveAll();


    public IStorageEntryContainer GetContainer(Guid containerId);
    public IStorageEntryContainer GetContainer(Type containerType);
    public IStorageEntryContainer[] GetContainers();
    public void CreateContainer(Guid containerId, Func<IStorageEntryContainerBuilder, IStorageEntryContainer> builder);
    public bool RemoveContainer(Guid containerId);
    public void RemoveAllContainers();

    public Task SaveStorageEntriesAsync(Guid containerId);
    public Task SaveAllAsync();
}

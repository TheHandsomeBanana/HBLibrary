using HBLibrary.Core.Extensions;
using HBLibrary.Interface.IO.Storage;
using HBLibrary.Interface.IO.Storage.Builder;
using HBLibrary.Interface.IO.Storage.Container;
using HBLibrary.Interface.IO.Storage.Entries;
using HBLibrary.IO.Storage.Builder;


namespace HBLibrary.IO.Storage;
public class ApplicationStorage : IApplicationStorage {
    internal Dictionary<Guid, IStorageEntryContainer> Containers { get; set; } = [];
    public string BasePath { get; }
    public Guid DefaultContainerId { get; }
    public IStorageEntryContainer DefaultContainer => Containers[DefaultContainerId];

    public ApplicationStorage(string basePath) {
        BasePath = basePath;
        DefaultContainerId = basePath.ToGuid();

        IStorageEntryContainer defaultContainer = new StorageEntryContainerBuilder(basePath)
            .ConfigureFileServices(fs => {
                fs.UseFileService()
                .UseJsonFileService()
                .UseXmlFileService();
            })
            .Build();


        Containers.Add(DefaultContainerId, defaultContainer);
    }

    public static IApplicationStorageBuilder CreateBuilder(string basePath) {
        return new ApplicationStorageBuilder(basePath);
    }

    public IEnumerable<IStorageEntry> GetStorageEntries(Guid containerId) {
        if (!Containers.TryGetValue(containerId, out IStorageEntryContainer? container)) {
            return [];
        }

        return container.GetAll();
    }

    public void SaveStorageEntries(Guid containerId) {
        if (Containers.TryGetValue(containerId, out IStorageEntryContainer? container)) {
            container.Save();
        }
    }

    public IStorageEntryContainer GetContainer(Guid containerId) {
        return Containers[containerId];
    }

    public IStorageEntryContainer GetContainer(Type containerType) {
        return Containers[containerType.GUID];
    }

    public void SaveAll() {
        foreach (IStorageEntryContainer container in Containers.Values) {
            container.Save();
        }
    }

    public void CreateContainer(Guid containerId, Func<IStorageEntryContainerBuilder, IStorageEntryContainer> builder) {
        if (Containers.ContainsKey(containerId)) {
            throw new InvalidOperationException($"Container with id {containerId} already created");
        }

        IStorageEntryContainer newContainer = builder(new StorageEntryContainerBuilder(BasePath));
        Containers.Add(containerId, newContainer);
    }

    public bool RemoveContainer(Guid containerId) {
        return Containers.Remove(containerId);
    }

    public void RemoveAllContainers() {
        Containers.Clear();
    }

    public async Task SaveAllAsync() {
        foreach (IStorageEntryContainer container in Containers.Values) {
            await container.SaveAsync();
        }
    }

    public Task SaveStorageEntriesAsync(Guid containerId) {
        if (!Containers.TryGetValue(containerId, out IStorageEntryContainer? container)) {
            throw new InvalidOperationException($"Container with id {containerId} not found");
        }

        return container.SaveAsync();
    }
}

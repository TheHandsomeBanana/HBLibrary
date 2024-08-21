using HBLibrary.Common.Extensions;
using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Builder;
using HBLibrary.Services.IO.Storage.Container;
using HBLibrary.Services.IO.Storage.Entries;
using HBLibrary.Services.IO.Xml;


namespace HBLibrary.Services.IO.Storage;
public class ApplicationStorage : IApplicationStorage {
    internal Dictionary<Guid, IStorageEntryContainer> Containers { get; set; } = [];
    public string BasePath { get; }
    public Guid DefaultContainerId { get; }

    public ApplicationStorage(string basePath) {
        BasePath = basePath;
        DefaultContainerId = basePath.ToGuid();

        IStorageEntryContainer defaultContainer = new StorageEntryContainerBuilder(basePath)
            .ConfigureFileServices(fs => {
                fs.UseFileService(() => new FileService())
                .UseJsonFileService(() => new JsonFileService())
                .UseXmlFileService(() => new XmlFileService());
            })
            .Build();

        Containers.Add(DefaultContainerId, defaultContainer);
    }

    public static IApplicationStorageBuilder CreateBuilder(string basePath) {
        return new ApplicationStorageBuilder(basePath);
    }

    public IStorageEntry? GetStorageEntry(Guid containerId, string filename) {
        if (!Containers.TryGetValue(containerId, out IStorageEntryContainer? container)) {
            return null;
        }

        return container.Get(filename);
    }

    public IStorageEntry? CreateStorageEntry(Guid containerId, string filename, StorageEntryContentType contentType) {
        if (!Containers.TryGetValue(containerId, out IStorageEntryContainer? container)) {
            return null;
        }

        return container.Create(filename, contentType);
    }

    public void AddOrUpdateStorageEntry(Guid containerId, string filename, object entry, StorageEntryContentType contentType) {
        if (!Containers.TryGetValue(containerId, out IStorageEntryContainer? container)) {
            throw new InvalidOperationException($"Container with id {containerId} not found");
        }

        container.AddOrUpdate(filename, entry, contentType);
    }

    public bool ContainsEntry(Guid containerId, string filename) {
        if (!Containers.TryGetValue(containerId, out IStorageEntryContainer? container)) {
            return false;
        }

        return container.Contains(filename);
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
}

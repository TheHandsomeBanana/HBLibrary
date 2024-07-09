using HBLibrary.Services.IO.Exceptions;
using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Builder;
using HBLibrary.Services.IO.Storage.Config;
using HBLibrary.Services.IO.Storage.Container;
using HBLibrary.Services.IO.Storage.Entries;
using HBLibrary.Services.IO.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using HBLibrary.Common.Extensions;

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
        if(!Containers.TryGetValue(containerId, out IStorageEntryContainer? container)) {
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

    public void AddOrUpdateStorageEntry(Guid containerId, string filename, StorageEntryContentType contentType) {
        throw new NotImplementedException();
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
}

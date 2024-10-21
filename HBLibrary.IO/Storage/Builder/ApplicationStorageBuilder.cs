using HBLibrary.Core.Extensions;
using HBLibrary.Interface.IO.Storage;
using HBLibrary.Interface.IO.Storage.Builder;
using HBLibrary.Interface.IO.Storage.Container;
using HBLibrary.IO.Storage;
using HBLibrary.IO.Storage.Container;

namespace HBLibrary.IO.Storage.Builder;
internal class ApplicationStorageBuilder : IApplicationStorageBuilder {
    private readonly string basePath;
    private readonly Dictionary<Guid, IStorageEntryContainer> containers = [];

    public ApplicationStorageBuilder(string basePath) {
        this.basePath = basePath;
    }


    public IApplicationStorage Build() {
        ApplicationStorage applicationStorage = new ApplicationStorage(basePath);

        foreach (KeyValuePair<Guid, IStorageEntryContainer> item in containers) {
            applicationStorage.Containers.Add(item.Key, item.Value);
        }

        return applicationStorage;
    }

    public IApplicationStorageBuilder AddContainer(Guid containerId, Func<IStorageEntryContainerBuilder, IStorageEntryContainer> containerBuilder) {
        IStorageEntryContainerBuilder builder = StorageEntryContainer.CreateBuilder(basePath);

        containers.Add(containerId, containerBuilder(builder));
        return this;
    }

    public IApplicationStorageBuilder AddContainer(Type type, Func<IStorageEntryContainerBuilder, IStorageEntryContainer> containerBuilder) {
        return AddContainer(type.GUID, containerBuilder);
    }

    public IApplicationStorageBuilder AddContainer(Func<IStorageEntryContainerBuilder, IStorageEntryContainer> containerBuilder) {
        IStorageEntryContainerBuilder builder = StorageEntryContainer.CreateBuilder(basePath);
        IStorageEntryContainer container = containerBuilder(builder);

        Guid containerId = container.BasePath.ToGuid();
        containers.Add(containerId, container);
        return this;
    }
}

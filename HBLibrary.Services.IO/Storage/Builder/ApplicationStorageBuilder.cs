using HBLibrary.Common.Extensions;
using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Container;
using HBLibrary.Services.IO.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Builder;
internal class ApplicationStorageBuilder : IApplicationStorageBuilder {
    private readonly string basePath;
    private readonly Dictionary<Guid, IStorageEntryContainer> containers = [];

    public ApplicationStorageBuilder(string basePath) {
        this.basePath = basePath;
    }


    public IApplicationStorage Build() {
        ApplicationStorage applicationStorage = new ApplicationStorage(basePath);

        if (containers.Count != 0) {
            applicationStorage.Containers = containers;
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

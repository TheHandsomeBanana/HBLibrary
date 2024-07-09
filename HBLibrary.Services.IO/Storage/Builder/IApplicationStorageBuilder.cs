using HBLibrary.Services.IO.Storage.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Builder;
public interface IApplicationStorageBuilder {
    IApplicationStorageBuilder AddContainer(Guid containerId, Func<IStorageEntryContainerBuilder, IStorageEntryContainer> containerBuilder);
    IApplicationStorageBuilder AddContainer(Func<IStorageEntryContainerBuilder, IStorageEntryContainer> containerBuilder);
    IApplicationStorageBuilder AddContainer(Type type, Func<IStorageEntryContainerBuilder, IStorageEntryContainer> containerBuilder);
    IApplicationStorage Build();
}

using HBLibrary.Services.IO.Storage.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Builder;
public interface IStorageEntryContainerBuilder {
    public IStorageEntryContainerBuilder ConfigureFileServices(Action<FileServiceContainer> action);
    public IStorageEntryContainerBuilder SetContainerPath(string path, bool relative = true);
    public IStorageEntryContainer Build();
}

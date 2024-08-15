using HBLibrary.Services.IO.Storage.Container;

namespace HBLibrary.Services.IO.Storage.Builder;
public interface IStorageEntryContainerBuilder {
    public IStorageEntryContainerBuilder ConfigureFileServices(Action<FileServiceContainer> action);
    public IStorageEntryContainerBuilder SetContainerPath(string path, bool relative = true);
    public IStorageEntryContainer Build();
}

using HBLibrary.Services.IO.Storage.Container;
using HBLibrary.Services.IO.Storage.Settings;

namespace HBLibrary.Services.IO.Storage.Builder;
public interface IStorageEntryContainerBuilder {
    public IStorageEntryContainerBuilder ConfigureFileServices(Action<FileServiceContainer> action);
    public IStorageEntryContainerBuilder SetContainerPath(string path, bool relative = true);
    public IStorageEntryContainerBuilder EnableCryptography(StorageContainerCryptography cryptography);
    public IStorageEntryContainer Build();
}

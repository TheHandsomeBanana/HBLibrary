using HBLibrary.Interface.IO.Storage.Container;
using HBLibrary.Interface.IO.Storage.Settings;

namespace HBLibrary.Interface.IO.Storage.Builder;
public interface IStorageEntryContainerBuilder {
    public IStorageEntryContainerBuilder ConfigureFileServices(Action<IFileServiceContainer> action);
    public IStorageEntryContainerBuilder SetContainerPath(string path, bool relative = true);
    public IStorageEntryContainerBuilder EnableCryptography(StorageContainerCryptography cryptography);
    public IStorageEntryContainer Build();
}

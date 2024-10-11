using HBLibrary.Services.IO.Storage.Container;
using HBLibrary.Services.IO.Storage.Settings;

namespace HBLibrary.Services.IO.Storage.Builder;
internal class StorageEntryContainerBuilder : IStorageEntryContainerBuilder {
    private string basePath;
    private readonly FileServiceContainer fileServices = new FileServiceContainer();
    private StorageContainerCryptography? storageContainerCryptography;

    public StorageEntryContainerBuilder(string basePath) {
        this.basePath = basePath;
    }

    public IStorageEntryContainer Build() {
        StorageEntryContainer container = new StorageEntryContainer(basePath, fileServices, storageContainerCryptography);
        return container;
    }

    public IStorageEntryContainerBuilder SetContainerPath(string path, bool relative) {
        if (relative) {
            basePath = Path.Combine(basePath, path);
        }
        else {
            basePath = path;
        }
        return this;
    }

    public IStorageEntryContainerBuilder ConfigureFileServices(Action<FileServiceContainer> action) {
        action(fileServices);
        return this;
    }

    public IStorageEntryContainerBuilder EnableCryptography(StorageContainerCryptography cryptography) {
        this.storageContainerCryptography = cryptography;
        return this;
    }
}

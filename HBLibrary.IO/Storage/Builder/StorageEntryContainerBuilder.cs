using HBLibrary.Interface.Core.ChangeTracker;
using HBLibrary.Interface.IO.Storage.Builder;
using HBLibrary.Interface.IO.Storage.Container;
using HBLibrary.Interface.IO.Storage.Settings;
using HBLibrary.IO.Storage.Container;

namespace HBLibrary.IO.Storage.Builder;
internal class StorageEntryContainerBuilder : IStorageEntryContainerBuilder {
    private string basePath;
    private readonly IFileServiceContainer fileServices = new FileServiceContainer();
    private StorageContainerCryptography? storageContainerCryptography;
    private IChangeTracker? changeTracker;

    public StorageEntryContainerBuilder(string basePath) {
        this.basePath = basePath;
    }

    public IStorageEntryContainer Build() {
        StorageEntryContainer container = new StorageEntryContainer(basePath) {
            FileServices = fileServices,
            Cryptography = storageContainerCryptography,
            ChangeTracker = changeTracker
        };

        container.InitEntries();
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

    public IStorageEntryContainerBuilder ConfigureFileServices(Action<IFileServiceContainer> action) {
        action(fileServices);
        return this;
    }

    public IStorageEntryContainerBuilder EnableCryptography(StorageContainerCryptography cryptography) {
        storageContainerCryptography = cryptography;
        return this;
    }

    public IStorageEntryContainerBuilder EnableChangeTracker(IChangeTracker changeTracker) {
        this.changeTracker = changeTracker;
        return this;
    }
}

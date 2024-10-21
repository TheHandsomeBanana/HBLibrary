using HBLibrary.Interface.IO.Json;
using HBLibrary.Interface.IO.Storage.Entries;
using HBLibrary.Interface.IO.Storage.Settings;
using HBLibrary.Interface.IO.Xml;
using System.Diagnostics.CodeAnalysis;

namespace HBLibrary.Interface.IO.Storage.Container;
public interface IStorageEntryContainer {
    string BasePath { get; }
    public IFileService? FileService { get; }
    public IJsonFileService? JsonFileService { get; }
    public IXmlFileService? XmlFileService { get; }
    public StorageContainerCryptography? Cryptography { get; }

    public IStorageEntry? this[string filename] { get; set; }
    public IStorageEntry? Get(string filename);
    public bool TryGet(string filename, [NotNullWhen(true)] out IStorageEntry? entry);
    public bool Contains(string filename);

    public IEnumerable<IStorageEntry> GetAll();

    public IStorageEntry Create(string filename, StorageEntryContentType contentType, StorageEntrySettings? settings = null);
    public void AddOrUpdate(string filename, object entry, StorageEntryContentType contentType, StorageEntrySettings? settings = null);

    public void Delete(string filename);

    public void Save();

    public Task SaveAsync();
}

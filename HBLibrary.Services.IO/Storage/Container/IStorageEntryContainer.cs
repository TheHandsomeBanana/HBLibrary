using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Entries;
using HBLibrary.Services.IO.Storage.Settings;
using HBLibrary.Services.IO.Xml;
using System.Diagnostics.CodeAnalysis;

namespace HBLibrary.Services.IO.Storage.Container;
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

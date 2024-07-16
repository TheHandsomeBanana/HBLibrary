using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Entries;
using HBLibrary.Services.IO.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Container;
public interface IStorageEntryContainer {
    string BasePath { get; }
    public IFileService? FileService { get; }
    public IJsonFileService? JsonFileService { get; }
    public IXmlFileService? XmlFileService { get; }

    public IStorageEntry? this[string filename] { get; set; }
    public IStorageEntry? Get(string filename);
    public bool TryGet(string filename, out IStorageEntry? entry);
    public bool Contains(string filename);

    public IEnumerable<IStorageEntry> GetAll();

    public IStorageEntry Create(string filename, StorageEntryContentType contentType);
    public void AddOrUpdate(string filename, object entry, StorageEntryContentType contentType);
    public void AddOrUpdate(IStorageEntry value);

    public void Delete(string filename);

    public void Save();
}

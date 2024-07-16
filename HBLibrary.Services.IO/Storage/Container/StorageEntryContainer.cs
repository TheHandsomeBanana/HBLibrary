using HBLibrary.Services.IO.Archiving.WinRAR.Commands;
using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Builder;
using HBLibrary.Services.IO.Storage.Config;
using HBLibrary.Services.IO.Storage.Entries;
using HBLibrary.Services.IO.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Container;
public class StorageEntryContainer : IStorageEntryContainer {
    private const string EXTENSION = ".se";

    private readonly Dictionary<string, IStorageEntry> entries = [];
    private readonly StorageContainerConfig config;
    internal FileServiceContainer? FileServices { get; set; }

    public string BasePath { get; }
    public IFileService? FileService => FileServices?.FileService;
    public IJsonFileService? JsonFileService => FileServices?.JsonFileService;
    public IXmlFileService? XmlFileService => FileServices?.XmlFileService;

    public StorageEntryContainer(string basePath, FileServiceContainer? fileServices = null) {
        this.BasePath = basePath;
        this.FileServices = fileServices;

        Directory.CreateDirectory(basePath);

        config = StorageContainerConfig.GetConfig(basePath) ?? StorageContainerConfig.CreateNew(basePath);
        InitEntries();
    }

    public static IStorageEntryContainerBuilder CreateBuilder(string basePath) {
        return new StorageEntryContainerBuilder(basePath);
    }


    private void InitEntries() {
        foreach(KeyValuePair<string, ContainerEntry> containerEntry in config.Entries) {
            string path = Path.Combine(this.BasePath, containerEntry.Key + EXTENSION);

            Create(path, containerEntry.Value);
        }
    }

    public IStorageEntry? this[string filename] {
        get => Get(filename);
        set {
            if(value is null) throw new ArgumentNullException(nameof(value));
            
            entries[filename] = value;
        }
    }

    public void AddOrUpdate(IStorageEntry value) {
        this[value.Filename] = value;
    }

    public IStorageEntry? Get(string filename) {
        string path = Path.Combine(this.BasePath, filename + EXTENSION);

        if (entries.TryGetValue(path, out IStorageEntry? entry)) {
            return entry;
        }

        return null;
    }

    public bool TryGet(string filename, out IStorageEntry? entry) {
        string path = Path.Combine(this.BasePath, filename + EXTENSION);

        return entries.TryGetValue(path, out entry);
    }

    public IEnumerable<IStorageEntry> GetAll() {
        return entries.Values;
    }

    public IStorageEntry Create(string filename, StorageEntryContentType contentType) {
        string path = Path.Combine(this.BasePath, filename + EXTENSION);

        if(!config.Entries.TryGetValue(filename, out ContainerEntry? containerEntry)) {
            containerEntry = new ContainerEntry(contentType);
            config.Entries.Add(filename, containerEntry);
        }

        return Create(path, containerEntry);
    }

    public void AddOrUpdate(string filename, object entry, StorageEntryContentType contentType) {
        string path = Path.Combine(this.BasePath, filename + EXTENSION);

        // Update
        if (entries.TryGetValue(path, out IStorageEntry? storageEntry)) {
            if(storageEntry.ContentType != contentType) {
                throw new InvalidOperationException($"Content type {contentType} does not match found entry content type {storageEntry.ContentType}.");
            }

            storageEntry.Set(entry);
            return;
        }

        // Add
        storageEntry = Create(filename, contentType);
        storageEntry.Set(entry);
        entries[path] = storageEntry;   
    }

    public bool Contains(string filename) {
        return entries.ContainsKey(filename);
    }

    public void Save() {
        foreach(IStorageEntry entry in entries.Values) {
            entry.Save();
        }

        config.Save();
    }

    private IStorageEntry Create(string filename, ContainerEntry containerEntry) {
        switch(containerEntry.ContentType) {
            case StorageEntryContentType.Json:
                if(JsonFileService is null) {
                    throw new InvalidOperationException($"{nameof(JsonFileService)} is null.");
                }

                StorageJsonEntry jsonEntry = new StorageJsonEntry(JsonFileService, filename);
                entries[filename] = jsonEntry;
                return jsonEntry;

            case StorageEntryContentType.Xml:
                if(XmlFileService is null) {
                    throw new InvalidOperationException($"{nameof(JsonFileService)} is null.");
                }

                StorageXmlEntry xmlEntry = new StorageXmlEntry(XmlFileService, filename);
                entries[filename] = xmlEntry;
                return xmlEntry;

            case StorageEntryContentType.Csv:
                StorageCsvEntry csvEntry = new StorageCsvEntry(filename);
                entries[filename] = csvEntry;
                return csvEntry;
            case StorageEntryContentType.Text:
                if(FileService is null) {
                    throw new InvalidOperationException($"{nameof(JsonFileService)} is null.");
                }

                StorageTextEntry textEntry = new StorageTextEntry(FileService, filename);
                entries[filename] = textEntry;
                return textEntry;

            default:
                throw new NotSupportedException(containerEntry.ContentType.ToString());
        }
    }

    public void Delete(string filename) {
        string path = Path.Combine(this.BasePath, filename + EXTENSION);

        if (!entries.ContainsKey(path)) {
            throw new InvalidOperationException($"Container does not contain entry with {path}.");
        }

        entries.Remove(path);
        config.Entries.Remove(filename);
        File.Delete(path);
    }
}

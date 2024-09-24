using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Builder;
using HBLibrary.Services.IO.Storage.Config;
using HBLibrary.Services.IO.Storage.Entries;
using HBLibrary.Services.IO.Storage.Settings;
using HBLibrary.Services.IO.Xml;
using System.Diagnostics.CodeAnalysis;

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
        foreach (KeyValuePair<string, ContainerEntry> containerEntry in config.Entries) {
            string path = Path.Combine(this.BasePath, containerEntry.Key + EXTENSION);

            Create(path, containerEntry.Value);
        }
    }

    public IStorageEntry? this[string filename] {
        get => Get(filename);
        set {
            if (value is null) {
                throw new ArgumentNullException(nameof(value));
            }

            entries[filename] = value;
        }
    }



    public IStorageEntry? Get(string filename) {
        string path = Path.Combine(this.BasePath, filename + EXTENSION);

        if (entries.TryGetValue(path, out IStorageEntry? entry)) {
            return entry;
        }

        return null;
    }

    public bool TryGet(string filename, [NotNullWhen(true)] out IStorageEntry? entry) {
        string path = Path.Combine(this.BasePath, filename + EXTENSION);

        return entries.TryGetValue(path, out entry);
    }

    public IEnumerable<IStorageEntry> GetAll() {
        return entries.Values;
    }

    public IStorageEntry Create(string filename, StorageEntryContentType contentType, StorageEntrySettings? settings = null) {

        string path = Path.Combine(this.BasePath, filename + EXTENSION);

        if (!config.Entries.TryGetValue(filename, out ContainerEntry? containerEntry)) {
            settings ??= StorageEntrySettings.CreateDefault();

            containerEntry = new ContainerEntry(contentType, settings);
            config.Entries.Add(filename, containerEntry);
        }

        return Create(path, containerEntry);
    }

    public void AddOrUpdate(string filename, object entry, StorageEntryContentType contentType, StorageEntrySettings? settings = null) {
        string path = Path.Combine(this.BasePath, filename + EXTENSION);

        // Update
        if (entries.TryGetValue(path, out IStorageEntry? storageEntry)) {
            if (storageEntry.ContentType != contentType) {
                throw new InvalidOperationException($"Content type {contentType} does not match found entry content type {storageEntry.ContentType}.");
            }

            if (settings is not null) {
                storageEntry.Settings = settings;
            }

            storageEntry.Set(entry);
            return;
        }

        // Add
        storageEntry = Create(filename, contentType, settings);
        storageEntry.Set(entry);
        entries[path] = storageEntry;
    }

    public bool Contains(string filename) {
        return entries.ContainsKey(filename);
    }

    public void Save() {
        foreach (IStorageEntry entry in entries.Values) {
            entry.Save();
        }

        config.Save();
    }

    private IStorageEntry Create(string filename, ContainerEntry containerEntry) {
        switch (containerEntry.ContentType) {
            case StorageEntryContentType.Json:
                if (JsonFileService is null) {
                    throw new InvalidOperationException($"{nameof(JsonFileService)} is null.");
                }

                StorageJsonEntry jsonEntry = new StorageJsonEntry(JsonFileService, filename, containerEntry.Settings);
                entries[filename] = jsonEntry;
                return jsonEntry;

            case StorageEntryContentType.Xml:
                if (XmlFileService is null) {
                    throw new InvalidOperationException($"{nameof(XmlFileService)} is null.");
                }

                StorageXmlEntry xmlEntry = new StorageXmlEntry(XmlFileService, filename, containerEntry.Settings);
                entries[filename] = xmlEntry;
                return xmlEntry;

            case StorageEntryContentType.Csv:
                StorageCsvEntry csvEntry = new StorageCsvEntry(filename, containerEntry.Settings);
                entries[filename] = csvEntry;
                return csvEntry;
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

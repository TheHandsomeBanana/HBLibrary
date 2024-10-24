using HBLibrary.Interface.Core.ChangeTracker;
using HBLibrary.Interface.IO;
using HBLibrary.Interface.IO.Json;
using HBLibrary.Interface.IO.Storage.Builder;
using HBLibrary.Interface.IO.Storage.Container;
using HBLibrary.Interface.IO.Storage.Entries;
using HBLibrary.Interface.IO.Storage.Settings;
using HBLibrary.Interface.IO.Xml;
using HBLibrary.IO.Storage.Builder;
using HBLibrary.IO.Storage.Config;
using HBLibrary.IO.Storage.Entries;
using System.Diagnostics.CodeAnalysis;

namespace HBLibrary.IO.Storage.Container;
public class StorageEntryContainer : IStorageEntryContainer {
    private const string EXTENSION = ".se";

    private readonly Dictionary<string, IStorageEntry> entries = [];
    private readonly StorageContainerConfig config;
    public StorageContainerCryptography? Cryptography { get; init; }
    internal IFileServiceContainer? FileServices { get; init; }

    public string BasePath { get; }
    public IFileService? FileService => FileServices?.FileService;
    public IJsonFileService? JsonFileService => FileServices?.JsonFileService;
    public IXmlFileService? XmlFileService => FileServices?.XmlFileService;
    public IChangeTracker? ChangeTracker { get; init; }

    public StorageEntryContainer(string basePath) {
        BasePath = basePath;
        Directory.CreateDirectory(basePath);
        config = StorageContainerConfig.GetConfig(basePath) ?? StorageContainerConfig.CreateNew(basePath);
    }

    public static IStorageEntryContainerBuilder CreateBuilder(string basePath) {
        return new StorageEntryContainerBuilder(basePath);
    }


    public void InitEntries() {
        foreach (KeyValuePair<string, ContainerEntry> containerEntry in config.Entries) {
            string path = Path.Combine(BasePath, containerEntry.Key + EXTENSION);

            if (containerEntry.Value.Settings.EncryptionEnabled && Cryptography is not null) {
                containerEntry.Value.Settings.ContainerCryptography = Cryptography;
            }

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
        string path = Path.Combine(BasePath, filename + EXTENSION);

        if (entries.TryGetValue(path, out IStorageEntry? entry)) {
            return entry;
        }

        return null;
    }

    public bool TryGet(string filename, [NotNullWhen(true)] out IStorageEntry? entry) {
        string path = Path.Combine(BasePath, filename + EXTENSION);

        return entries.TryGetValue(path, out entry);
    }

    public IEnumerable<IStorageEntry> GetAll() {
        return entries.Values;
    }

    public IStorageEntry Create(string filename, StorageEntryContentType contentType, StorageEntrySettings? settings = null) {

        string path = Path.Combine(BasePath, filename + EXTENSION);

        if (!config.Entries.TryGetValue(filename, out ContainerEntry? containerEntry)) {
            settings ??= StorageEntrySettings.CreateDefault();

            containerEntry = new ContainerEntry(contentType, settings);
            config.Entries.Add(filename, containerEntry);
        }

        return Create(path, containerEntry);
    }

    public void AddOrUpdate(string filename, object entry, StorageEntryContentType contentType, StorageEntrySettings? settings = null) {
        if (Cryptography is not null) {
            settings ??= StorageEntrySettings.CreateDefault();
            settings.EncryptionEnabled = true;
            settings.ContainerCryptography = Cryptography;
        }

        string path = Path.Combine(BasePath, filename + EXTENSION);

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
            ChangeTracker?.SaveChanges(entry);
        }

        config.Save();
    }

    public async Task SaveAsync() {
        foreach (IStorageEntry entry in entries.Values) {
            await entry.SaveAsync();
            ChangeTracker?.SaveChanges(entry);
        }

        await config.SaveAsync();
    }

    private IStorageEntry Create(string filename, ContainerEntry containerEntry) {
        switch (containerEntry.ContentType) {
            case StorageEntryContentType.Json:
                if (JsonFileService is null) {
                    throw new InvalidOperationException($"{nameof(JsonFileService)} is null.");
                }

                StorageJsonEntry jsonEntry = new StorageJsonEntry(JsonFileService, filename, containerEntry.Settings, ChangeTracker);
                ChangeTracker?.Track(jsonEntry);

                entries[filename] = jsonEntry;
                return jsonEntry;

            case StorageEntryContentType.Xml:
                if (XmlFileService is null) {
                    throw new InvalidOperationException($"{nameof(XmlFileService)} is null.");
                }

                StorageXmlEntry xmlEntry = new StorageXmlEntry(XmlFileService, filename, containerEntry.Settings, ChangeTracker);
                ChangeTracker?.Track(xmlEntry);

                entries[filename] = xmlEntry;
                return xmlEntry;

            case StorageEntryContentType.Csv:
                StorageCsvEntry csvEntry = new StorageCsvEntry(filename, containerEntry.Settings, ChangeTracker);
                ChangeTracker?.Track(csvEntry);

                entries[filename] = csvEntry;
                return csvEntry;
            default:
                throw new NotSupportedException(containerEntry.ContentType.ToString());
        }
    }

    public void Delete(string filename) {
        string path = Path.Combine(BasePath, filename + EXTENSION);

        if (!entries.TryGetValue(path, out IStorageEntry? value)) {
            throw new InvalidOperationException($"Container does not contain entry with {path}.");
        }

        ChangeTracker?.Untrack(value);
        if (value.CurrentEntryType is not null) {
            object? internalValue = value.Get(value.CurrentEntryType);
            if (internalValue is ITrackable notifyTrackableChanged) {
                ChangeTracker?.Untrack(notifyTrackableChanged);
            }
        }

        entries.Remove(path);
        config.Entries.Remove(filename);
        File.Delete(path);
    }

    public void Dispose() {
        ChangeTracker?.Dispose();
        entries.Clear();
    }
}

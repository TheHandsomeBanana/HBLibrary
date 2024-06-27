using CsvHelper;
using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Container;
using HBLibrary.Services.IO.Storage.Entries;
using HBLibrary.Services.IO.Xml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using Unity;
using static Unity.Storage.RegistrationSet;

namespace HBLibrary.Services.IO.Storage;
public class ApplicationStorage : IApplicationStorage {
    public const string EXTENSION = "";

    public string? BasePath { get; set; }
    public IStorageEntryContainer? Container { get; private set; }

    [Dependency]
    public IJsonFileService? JsonFileService { get; set; }
    [Dependency]
    public IXmlFileService? XmlFileService { get; set; }

    public ApplicationStorage() { }

    public IStorageEntry<T> GetStorageEntry<T>(string filename, StorageEntryType entryType, bool lazy = true) where T : class {
        string path = BasePath is not null
            ? Path.Combine(BasePath, filename + EXTENSION)
            : filename + EXTENSION;

        if (Container?.TryGetEntry(path, out IStorageEntry<T>? entry) ?? false) {
            return entry!;
        }

        switch (entryType) {
            case StorageEntryType.Json:
                if (JsonFileService is null) {
                    throw new MissingMemberException(nameof(JsonFileService));
                }

                return new StorageJsonEntry<T>(JsonFileService, path, lazy);
            case StorageEntryType.Xml:
                return new StorageXmlEntry<T>(path, lazy);
            default:
                throw new NotSupportedException(entryType.ToString());
        }
    }

    public void SaveStorageEntry<T>(T entry, string filename, StorageEntryType entryType) where T : class {
        string path = BasePath is not null
            ? Path.Combine(BasePath, filename + EXTENSION)
            : filename + EXTENSION;

        switch (entryType) {
            case StorageEntryType.Json:
                if (JsonFileService is null) {
                    throw new MissingMemberException(nameof(JsonFileService));
                }

                JsonFileService.WriteJson(FileSnapshot.Create(path, true), entry, new JsonSerializerOptions() { WriteIndented = true });

                Container?.AddEntry(new StorageJsonEntry<T>(JsonFileService, entry, path));
                return;
            case StorageEntryType.Xml:
                if (XmlFileService is null) {
                    throw new MissingMemberException(nameof(XmlFileService));
                }

                XmlFileService.WriteXml(FileSnapshot.Create(path, true), entry);

                Container?.AddEntry(new StorageXmlEntry<T>(entry, path));
                return;
        }

        throw new NotSupportedException(entryType.ToString());
    }

    public IStorageListEntry<T> GetStorageListEntry<T>(string filename, StorageEntryType entryType, bool lazy = true) where T : class {
        string path = BasePath is not null
                    ? Path.Combine(BasePath, filename + EXTENSION)
                    : filename + EXTENSION;

        if (Container?.TryGetListEntry(path, out IStorageListEntry<T>? entry) ?? false) {
            return entry!;
        }

        switch(entryType) {
            case StorageEntryType.Json:
                if (JsonFileService is null) {
                    throw new MissingMemberException(nameof(JsonFileService));
                }

                return new StorageJsonListEntry<T>(JsonFileService, path, lazy);
            case StorageEntryType.Xml:
                return new StorageXmlListEntry<T>(path, lazy);
            case StorageEntryType.Csv:
                return new StorageCsvEntry<T>(path, lazy);
            default:
                throw new NotSupportedException(entryType.ToString());
        }
    }

    public void SaveStorageListEntry<T>(T[] entry, string filename, StorageEntryType entryType) where T : class {
        string path = BasePath is not null
                    ? Path.Combine(BasePath, filename + EXTENSION)
                    : filename + EXTENSION;

        switch (entryType) {
            case StorageEntryType.Json:
                if (JsonFileService is null) {
                    throw new MissingMemberException(nameof(JsonFileService));
                }

                JsonFileService.WriteJson(FileSnapshot.Create(path, true), entry, new JsonSerializerOptions() { WriteIndented = true });

                Container?.AddEntry(new StorageJsonListEntry<T>(JsonFileService, entry, path));
                return;

            case StorageEntryType.Xml:
                XmlFileService xmlFileService = new XmlFileService();
                xmlFileService.WriteXml(FileSnapshot.Create(path, true), entry);

                Container?.AddEntry(new StorageXmlListEntry<T>(entry, path));
                return;

            case StorageEntryType.Csv:
                using (StreamWriter sw = new StreamWriter(path)) {
                    using (CsvWriter csvWriter = new CsvWriter(sw, CultureInfo.InvariantCulture)) {
                        csvWriter.WriteRecords(entry);
                    }
                }

                Container?.AddEntry(new StorageCsvEntry<T>(entry, filename));
                return;
        }

        throw new NotSupportedException(entryType.ToString());
    }

    public bool TryGetStorageEntry<T>(string filename, out IStorageEntry<T>? entry) where T : class {
        string path = BasePath is not null
            ? Path.Combine(BasePath, filename + EXTENSION)
            : filename + EXTENSION;

        if (Container is null) {
            entry = null;
            return false;
        }

        return Container.TryGetEntry(path, out entry);
    }

    public bool TryGetStorageEntry<T>(out IStorageEntry<T>? entry) where T : class {
        if (BasePath is null) {
            entry = null;
            return false;
        }

        return TryGetStorageEntry(typeof(T).GUID.ToString(), out entry);
    }

    public IStorageEntry<T> GetStorageEntry<T>(StorageEntryType entryType, bool lazy = true) where T : class {
        if (BasePath is null) {
            throw new MissingMemberException(BasePath);
        }

        return GetStorageEntry<T>(typeof(T).GUID.ToString(), entryType, lazy);
    }

    public void SaveStorageEntry<T>(T entry, StorageEntryType entryType) where T : class {
        if (BasePath is null) {
            throw new MissingMemberException(BasePath);
        }

        SaveStorageEntry<T>(entry, typeof(T).GUID.ToString(), entryType);
    }

    public IStorageListEntry<T> GetStorageListEntry<T>(StorageEntryType entryType, bool lazy = true) where T : class {
        if (BasePath is null) {
            throw new MissingMemberException(BasePath);
        }

        return GetStorageListEntry<T>(typeof(T).GUID.ToString(), entryType, lazy);
    }

    public void SaveStorageListEntry<T>(T[] entry, StorageEntryType entryType) where T : class {
        if (BasePath is null) {
            throw new MissingMemberException(BasePath);
        }

        SaveStorageListEntry<T>(entry, typeof(T).GUID.ToString(), entryType);
    }

    public void UseContainer() {
        Container = new StorageEntryContainer();
    }
}

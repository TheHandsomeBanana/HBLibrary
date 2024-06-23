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
using System.Threading.Tasks;
using static Unity.Storage.RegistrationSet;

namespace HBLibrary.Services.IO.Storage;
public class ApplicationStorage : IApplicationStorage {
    public string? BasePath { get; set; }
    public IStorageEntryContainer? Container { get; }

    public ApplicationStorage(string? basePath = null, bool useContainer = true) {
        BasePath = basePath;

        if (useContainer) {
            Container = new StorageEntryContainer();
        }
    }

    public IStorageEntry<T> GetStorageEntry<T>(string filename, StorageEntryType entryType, bool lazy = true) {
        string path = BasePath is not null
            ? Path.Combine(BasePath, filename)
            : filename;

        if (Container is not null) {
            if (Container.TryGetEntry(path, out IStorageEntry<T>? entry)) {
                return entry!;
            }
        }

        return entryType switch {
            StorageEntryType.Json => new StorageJsonEntry<T>(path, lazy),
            StorageEntryType.Xml => new StorageXmlEntry<T>(path, lazy),
            _ => throw new NotSupportedException(entryType.ToString()),
        };
    }

    public void SaveStorageEntry<T>(T entry, string filename, StorageEntryType entryType) {
        string path = BasePath is not null
            ? Path.Combine(BasePath, filename)
            : filename;

        switch (entryType) {
            case StorageEntryType.Json:
                JsonFileService jsonFileService = new JsonFileService();
                jsonFileService.WriteJson(FileSnapshot.Create(path, true), entry, new JsonSerializerOptions() { WriteIndented = true });

                Container?.AddEntry(new StorageJsonEntry<T>(entry, path));
                return;
            case StorageEntryType.Xml:
                XmlFileService xmlFileService = new XmlFileService();
                xmlFileService.WriteXml(FileSnapshot.Create(path, true), entry);

                Container?.AddEntry(new StorageXmlEntry<T>(entry, path));
                return;
        }

        throw new NotSupportedException(entryType.ToString());
    }

    public IStorageListEntry<T> GetStorageListEntry<T>(string filename, StorageEntryType entryType, bool lazy = true) {
        string path = BasePath is not null
                    ? Path.Combine(BasePath, filename)
                    : filename;

        if (Container is not null) {
            if (Container.TryGetListEntry(path, out IStorageListEntry<T>? entry)) {
                return entry!;
            }
        }

        return entryType switch {
            StorageEntryType.Csv => new StorageCsvEntry<T>(path, lazy),
            _ => throw new NotSupportedException(entryType.ToString()),
        };
    }

    public void SaveStorageListEntry<T>(IEnumerable<T> entries, string filename, StorageEntryType entryType) {
        string path = BasePath is not null
                    ? Path.Combine(BasePath, filename)
                    : filename;

        switch (entryType) {
            case StorageEntryType.Json:
                JsonFileService jsonFileService = new JsonFileService();
                jsonFileService.WriteJson<T[]>(FileSnapshot.Create(path, true), entries.ToArray(), new JsonSerializerOptions() { WriteIndented = true });
                Container?.AddEntry(new StorageJsonEntry<T>(entry, path));
                return;

            case StorageEntryType.Csv:
                using (StreamWriter sw = new StreamWriter(path)) {
                    using (CsvWriter csvWriter = new CsvWriter(sw, CultureInfo.InvariantCulture)) {
                        csvWriter.WriteRecords(entries);
                    }
                }

                Container?.AddEntry(new StorageCsvEntry<T>(entries.ToArray(), filename));
                return;
        }

        throw new NotSupportedException(entryType.ToString());
    }

    public bool TryGetStorageEntry<T>(string filename, out IStorageEntry<T>? entry) {
        string path = BasePath is not null
            ? Path.Combine(BasePath, filename)
            : filename;

        if (Container is null) {
            entry = null;
            return false;
        }

        return Container.TryGetEntry(path, out entry);
    }

    
}

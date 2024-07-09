using HBLibrary.Services.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageJsonEntry : IStorageEntry {
    private object? entry;
    private readonly IJsonFileService jsonService;
    public string Filename { get; }

    public StorageEntryContentType ContentType => StorageEntryContentType.Json;

    public Type? CurrentEntryType => entry?.GetType();

    internal StorageJsonEntry(IJsonFileService jsonService, string filename) {
        this.jsonService = jsonService;
        this.Filename = filename;
    }

    public object? Get(Type type) {
        try {
            if (entry is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                entry = jsonService.ReadJson(type, file!);
            }

            return entry;
        }
        catch {
            return default;
        }
    }

    public void Set(object value) {
        entry = value;
    }

    public void Save(Type type) {
        if(entry is null) {
            throw new InvalidOperationException($"{nameof(entry)} is null.");
        }

        if (entry.GetType() != type) {
            throw new InvalidOperationException("Cannot save, entry does not equal given type.");
        }

        jsonService.WriteJson(type, FileSnapshot.Create(Filename, true), entry);
    }

    public void Save() {
        if (CurrentEntryType is null) {
            throw new InvalidOperationException($"{nameof(entry)} is null.");
        }

        Save(CurrentEntryType);
    }
}

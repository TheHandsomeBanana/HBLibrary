using CsvHelper;
using HBLibrary.Services.IO.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageJsonEntry<T> : StorageEntry, IStorageEntry<T> {
    private T? entry;
    private readonly IJsonFileService jsonService;
    internal StorageJsonEntry(IJsonFileService jsonService, string filename, bool lazy) : base(filename, lazy) {
        this.jsonService = jsonService; 

        if (!lazy) {
            if (FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {

                entry = jsonService.ReadJson<T>(file!.Value);
            }
        }
    }

    internal StorageJsonEntry(IJsonFileService jsonService, T entry, string filename) : this(jsonService, filename, false) {
        this.entry = entry;
    }

    public T? Get() {
        if (!IsLoaded) {
            IsLoaded = true;

            if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                return default;
            }

            entry = jsonService.ReadJson<T>(file!.Value);
        }

        return entry!;
    }

    object IStorageEntry.Get() {
        return Get()!;
    }
}

internal class StorageJsonListEntry<T> : StorageEntry, IStorageListEntry<T> {
    private T[] entry = [];
    private readonly IJsonFileService jsonService;

    internal StorageJsonListEntry(IJsonFileService jsonService, string filename, bool lazy) : base(filename, lazy) {
        this.jsonService = jsonService;
        
        if (!lazy) {
            if (FileSnapshot.TryCreate(filename, out FileSnapshot? file)) {
                entry = jsonService.ReadJson<T[]>(file!.Value) ?? [];
            }
        }
    }

    internal StorageJsonListEntry(IJsonFileService jsonService, T[] entry, string filename) : this(jsonService, filename, false) {
        this.entry = entry;
    }

    public T[] Get() {
        if (!IsLoaded) {
            IsLoaded = true;

            if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                return [];
            }

            entry = jsonService.ReadJson<T[]>(file!.Value) ?? [];
        }

        return entry!;
    }

    public T? Get(int index) {
        if (!IsLoaded) {
            IsLoaded = true;

            if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                return default;
            }

            entry = jsonService.ReadJson<T[]>(file!.Value) ?? [];
            return entry.ElementAtOrDefault(index);
        }

        return entry.ElementAtOrDefault(index);
    }

    object IStorageEntry.Get() {
        return Get();
    }
}

using CsvHelper;
using HBLibrary.Services.IO.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageJsonEntry<T> : IStorageEntry<T> {
    private bool IsLoaded = false;
    private T? entry;
    public string Filename { get; }

    internal StorageJsonEntry(string filename, bool lazy) {
        this.Filename = filename;

        if (!lazy) {
            JsonFileService jsonService = new JsonFileService();
            entry = jsonService.ReadJson<T>(FileSnapshot.Create(this.Filename));
        }

        this.IsLoaded = !lazy;
    }

    internal StorageJsonEntry(T entry, string filename) {
        this.Filename = filename;
        this.entry = entry;
        IsLoaded = true;
    }

    public T Get() {
        if (!IsLoaded) {
            IsLoaded = true;

            JsonFileService jsonService = new JsonFileService();
            entry = jsonService.ReadJson<T>(FileSnapshot.Create(Filename));
        }

        return entry!;
    }

    object IStorageEntry.Get() {
        return Get()!;
    }
}

internal class StorageJsonListEntry<T> : StorageEntry, IStorageListEntry<T> {
    private T[] entry = [];

    internal StorageJsonListEntry(string filename, bool lazy) : base(filename, lazy) {
        if (!lazy) {
            JsonFileService jsonService = new JsonFileService();
            entry = jsonService.ReadJson<T[]>(FileSnapshot.Create(this.Filename)) ?? [];
        }
    }

    internal StorageJsonListEntry(T[] entry, string filename) : base(filename, false) {
        this.entry = entry;
    }

    public T[] Get() {
        if (!IsLoaded) {
            IsLoaded = true;

            JsonFileService jsonService = new JsonFileService();
            entry = jsonService.ReadJson<T[]>(FileSnapshot.Create(this.Filename)) ?? [];
        }

        return entry!;
    }

    public T? Get(int index) {
        if (!IsLoaded) {
            IsLoaded = true;
            JsonFileService jsonService = new JsonFileService();
            entry = jsonService.ReadJson<T[]>(FileSnapshot.Create(this.Filename)) ?? [];
            return entry.ElementAtOrDefault(index);
        }

        return entry.ElementAtOrDefault(index);
    }

    object IStorageEntry.Get() {
        return Get()!;
    }
}

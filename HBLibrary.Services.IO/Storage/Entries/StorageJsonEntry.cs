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
    private T[] entries = [];

    internal StorageJsonListEntry(string filename, bool lazy) : base(filename, lazy) {
        if (!lazy) {
            using StreamReader sr = new StreamReader(filename);
            using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

            entries = csvReader.GetRecords<T>().ToArray();
        }
    }

    internal StorageJsonListEntry(T[] entries, string filename) : base(filename, false) {
        this.entries = entries;
    }

    public T[] Get() {
        if (!IsLoaded) {
            IsLoaded = true;

            using StreamReader sr = new StreamReader(Filename);
            using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

            entries = csvReader.GetRecords<T>().ToArray();
        }

        return entries!;
    }

    public T? Get(int index) {
        if (!IsLoaded) {
            using StreamReader sr = new StreamReader(Filename);
            using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

            return csvReader.GetRecords<T>().ElementAtOrDefault(1);
        }

        return entries.ElementAtOrDefault(index);
    }

    object IStorageEntry.Get() {
        return Get()!;
    }
}

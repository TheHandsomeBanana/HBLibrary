using HBLibrary.Services.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageJsonEntry<T> : IStorageEntry<T> {
    private bool isLoaded = false;
    private T? entry;
    public string Filename { get; }

    internal StorageJsonEntry(string filename, bool lazy) {
        this.Filename = filename;

        if (!lazy) {
            JsonFileService jsonService = new JsonFileService();
            entry = jsonService.ReadJson<T>(FileSnapshot.Create(this.Filename));
        }

        this.isLoaded = !lazy;
    }

    internal StorageJsonEntry(T entry, string filename) {
        this.Filename = filename;
        this.entry = entry;
        isLoaded = true;
    }

    public T Get() {
        if (!isLoaded) {
            isLoaded = true;

            JsonFileService jsonService = new JsonFileService();
            entry = jsonService.ReadJson<T>(FileSnapshot.Create(Filename));
        }

        return entry!;
    }

    object IStorageEntry.Get() {
        return Get()!;
    }
}

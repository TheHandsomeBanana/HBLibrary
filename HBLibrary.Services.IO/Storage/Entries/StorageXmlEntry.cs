using CsvHelper;
using HBLibrary.Services.IO.Xml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageXmlEntry<T> : StorageEntry, IStorageEntry<T> {
    private T? entry;

    internal StorageXmlEntry(string filename, bool lazy) : base(filename, lazy) {
        if (!lazy) {
            XmlFileService jsonService = new XmlFileService();
            entry = jsonService.ReadXml<T>(FileSnapshot.Create(this.Filename));
        }
    }

    internal StorageXmlEntry(T entry, string filename) : base(filename, false){
        this.entry = entry;
    }

    public T Get() {
        if (!IsLoaded) {
            IsLoaded = true;

            XmlFileService jsonService = new XmlFileService();
            entry = jsonService.ReadXml<T>(FileSnapshot.Create(this.Filename));
        }

        return entry!;
    }

    object IStorageEntry.Get() {
        return Get()!;
    }
}

internal class StorageXmlListEntry<T> : StorageEntry, IStorageListEntry<T> {
    private T[] entries = [];

    internal StorageXmlListEntry(string filename, bool lazy) : base(filename, lazy) {
        if (!lazy) {
            XmlFileService jsonService = new XmlFileService();
            entries = jsonService.ReadXml<T[]>(FileSnapshot.Create(this.Filename)) ?? [];
        }
    }

    internal StorageXmlListEntry(T[] entries, string filename) : base(filename, false) {
        this.entries = entries;
    }

    public T[] Get() {
        if (!IsLoaded) {
            IsLoaded = true;

            XmlFileService jsonService = new XmlFileService();
            entries = jsonService.ReadXml<T[]>(FileSnapshot.Create(this.Filename)) ?? [];
        }

        return entries!;
    }

    public T? Get(int index) {
        if (!IsLoaded) {
            IsLoaded = true;
            XmlFileService jsonService = new XmlFileService();
            entries = jsonService.ReadXml<T[]>(FileSnapshot.Create(this.Filename)) ?? [];
            return entries.ElementAtOrDefault(1);
        }

        return entries.ElementAtOrDefault(index);
    }

    object IStorageEntry.Get() {
        return Get()!;
    }
}

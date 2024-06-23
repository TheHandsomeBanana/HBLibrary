using HBLibrary.Services.IO.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageXmlEntry<T> : IStorageEntry<T> {
    private bool isLoaded = false;
    private T? entry;
    public string Filename { get; }

    internal StorageXmlEntry(string filename, bool lazy) {
        this.Filename = filename;

        if (!lazy) {
            XmlFileService jsonService = new XmlFileService();
            entry = jsonService.ReadXml<T>(FileSnapshot.Create(this.Filename));
        }

        this.isLoaded = !lazy;
    }

    internal StorageXmlEntry(T entry, string filename) {
        this.Filename = filename;
        this.entry = entry;
        isLoaded = true;
    }

    public T Get() {
        if (!isLoaded) {
            isLoaded = true;

            XmlFileService jsonService = new XmlFileService();
            entry = jsonService.ReadXml<T>(FileSnapshot.Create(this.Filename));
        }

        return entry!;
    }

    object IStorageEntry.Get() {
        return Get()!;
    }
}

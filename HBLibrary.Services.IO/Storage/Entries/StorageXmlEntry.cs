using HBLibrary.Services.IO.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
public class StorageXmlEntry : IStorageEntry {
    private object? entry;
    private readonly IXmlFileService xmlService;
    public string Filename { get; }

    public StorageEntryContentType ContentType => StorageEntryContentType.Xml;

    public Type? CurrentEntryType => entry?.GetType();

    internal StorageXmlEntry(IXmlFileService xmlService, string filename) {
        this.xmlService = xmlService;
        this.Filename = filename;
    }

    public object? Get(Type type) {
        try {
            if (entry is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return null;
                }

                entry = xmlService.ReadXml(type, file!);
            }

            return entry;
        }
        catch {
            return null;
        }
    }

    public void Set(object value) {
        entry = value;
    }

    public void Save(Type type) {
        if (entry is null) {
            throw new MissingFieldException(nameof(entry));
        }

        if (entry.GetType() != type) {
            throw new InvalidOperationException("Cannot save, entry does not equal given type.");
        }

        xmlService.WriteXml(type, FileSnapshot.Create(Filename, true), entry);
    }

    public void Save() {
        if (CurrentEntryType is null) {
            throw new InvalidOperationException($"{nameof(entry)} is null.");
        }

        Save(CurrentEntryType);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
internal abstract class StorageEntry {
    protected bool IsLoaded { get; set; }
    public string Filename { get; protected set; }

    protected StorageEntry(string filename, bool lazy) {
        Filename = filename;
        IsLoaded = !lazy;
    }
}

public enum StorageEntryType {
    Csv,
    Json,
    Xml,
}

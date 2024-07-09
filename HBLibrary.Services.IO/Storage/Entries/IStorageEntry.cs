using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
public interface IStorageEntry {
    public string Filename { get; }
    public StorageEntryContentType ContentType { get; }
    public Type? CurrentEntryType { get; }

    public object? Get(Type type);
    public void Set(object value);
    public void Save(Type type);
    public void Save();
}

public enum StorageEntryContentType {
    Text,
    Csv,
    Json,
    Xml,
}

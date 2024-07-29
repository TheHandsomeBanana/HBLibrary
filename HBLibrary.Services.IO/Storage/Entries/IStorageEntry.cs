using HBLibrary.Services.IO.Storage.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
public interface IStorageEntry {
    public string Filename { get; }
    public StorageEntryContentType ContentType { get; }
    public StorageEntrySettings Settings { get; set; }
    public Type? CurrentEntryType { get; }

    public object? Get(Type type);
    public void Set(object value);
    public void Save(Type type);
    public void Save();

    public T? Get<T>();
    public void Set<T>(T value);
    public void Save<T>();
}

public enum StorageEntryContentType {
    Csv,
    Json,
    Xml,
}

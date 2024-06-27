using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Container;
using HBLibrary.Services.IO.Storage.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage;
public interface IApplicationStorage {
    public string? BasePath { get; }

    public IStorageEntry<T> GetStorageEntry<T>(string filename, StorageEntryType entryType, bool lazy = true) where T : class;
    public void SaveStorageEntry<T>(T entry, string filename, StorageEntryType entryType) where T : class;

    public IStorageListEntry<T> GetStorageListEntry<T>(string filename, StorageEntryType entryType, bool lazy = true) where T : class;
    public void SaveStorageListEntry<T>(T[] entry, string filename, StorageEntryType entryType) where T : class;


    public IStorageEntry<T> GetStorageEntry<T>(StorageEntryType entryType, bool lazy = true) where T : class;
    public void SaveStorageEntry<T>(T entry, StorageEntryType entryType) where T : class;
    public IStorageListEntry<T> GetStorageListEntry<T>(StorageEntryType entryType, bool lazy = true) where T : class;
    public void SaveStorageListEntry<T>(T[] entry, StorageEntryType entryType) where T : class;


    public void UseContainer();
    public IStorageEntryContainer? Container { get; }
    public bool TryGetStorageEntry<T>(string filename, out IStorageEntry<T>? entry) where T : class;
    public bool TryGetStorageEntry<T>(out IStorageEntry<T>? entry) where T : class;


    
}

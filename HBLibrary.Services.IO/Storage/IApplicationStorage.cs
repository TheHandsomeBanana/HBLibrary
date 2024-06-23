using HBLibrary.Services.IO.Storage.Container;
using HBLibrary.Services.IO.Storage.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage;
public interface IApplicationStorage {
    public IStorageEntry<T> GetStorageEntry<T>(string filename, StorageEntryType entryType, bool lazy = true);
    public void SaveStorageEntry<T>(T entry, string filename, StorageEntryType entryType);

    public IStorageListEntry<T> GetStorageListEntry<T>(string filename, StorageEntryType entryType, bool lazy = true);
    public void SaveStorageListEntry<T>(T[] entries, string filename, StorageEntryType entryType);

    public IStorageEntryContainer? Container { get; }
    public bool TryGetStorageEntry<T>(string filename, out IStorageEntry<T>? entry);
}

using HBLibrary.Services.IO.Storage.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Container;
internal class StorageEntryContainer : IStorageEntryContainer {
    private readonly Dictionary<string, IStorageEntry> entries = [];
    public IReadOnlyDictionary<string, IStorageEntry> Entries => entries;

    public void AddEntry(IStorageEntry entry) {
#if NET5_0_OR_GREATER
        entries.TryAdd(entry.Filename, entry);
#elif NET472_OR_GREATER
        if(!entries.ContainsKey(entry.Filename))
            entries.Add(entry.Filename, entry);
#endif
    }

    public bool TryGetEntry<T>(string filename, out IStorageEntry<T>? castedEntry) {
        bool hasValue = entries.TryGetValue(filename, out IStorageEntry? entry);
        castedEntry = entry as IStorageEntry<T>;
        return hasValue;
    }

    public bool TryGetListEntry<T>(string filename, out IStorageListEntry<T>? castedEntry) {
        bool hasValue = entries.TryGetValue(filename, out IStorageEntry? entry);
        castedEntry = entry as IStorageListEntry<T>;
        return hasValue;
    }
}

using HBLibrary.Services.IO.Storage.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Container;
public interface IStorageEntryContainer {
    public IReadOnlyDictionary<string, IStorageEntry> Entries { get; }
    public void AddEntry(IStorageEntry entry);
    public bool TryGetEntry<T>(string filename, out IStorageEntry<T>? entry);
    public bool TryGetListEntry<T>(string filename, out IStorageListEntry<T>? entry);
}

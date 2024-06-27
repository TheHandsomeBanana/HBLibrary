using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;


public interface IStorageEntry {
    public string Filename { get; }
    public object Get();
}

public interface IStorageEntry<T> : IStorageEntry {
    public new T? Get();
}

public interface IStorageListEntry<T> : IStorageEntry {
    public new T[] Get();
    public T? Get(int index);
}

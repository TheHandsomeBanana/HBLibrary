using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageCsvEntry<T> : StorageEntry, IStorageListEntry<T> {
    private T[] entry = [];

    internal StorageCsvEntry(string filename, bool lazy) : base(filename, lazy) {
        if (!lazy) {
            using StreamReader sr = new StreamReader(filename);
            using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

            entry = csvReader.GetRecords<T>().ToArray();
        }
    }

    internal StorageCsvEntry(T[] entries, string filename) : base(filename, false) {
        this.entry = entries;
    }

    public T[] Get() {
        if (!IsLoaded) {
            IsLoaded = true;

            using StreamReader sr = new StreamReader(Filename);
            using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

            entry = csvReader.GetRecords<T>().ToArray();
        }

        return entry!;
    }

    public T? Get(int index) {
        if (!IsLoaded) {
            using StreamReader sr = new StreamReader(Filename);
            using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

            return csvReader.GetRecords<T>().ElementAtOrDefault(1);
        }

        return entry.ElementAtOrDefault(index);
    }

    object IStorageEntry.Get() {
        return Get()!;
    }
}

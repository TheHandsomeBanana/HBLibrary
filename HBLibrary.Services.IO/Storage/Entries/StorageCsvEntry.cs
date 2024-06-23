using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageCsvEntry<T> : IStorageListEntry<T> {
    private bool isLoaded = false;
    private T[] entries = [];
    public string Filename { get; }

    internal StorageCsvEntry(string filename, bool lazy) {
        this.Filename = filename;

        if (!lazy) {
            using StreamReader sr = new StreamReader(filename);
            using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

            entries = csvReader.GetRecords<T>().ToArray();
        }

        this.isLoaded = !lazy;
    }

    internal StorageCsvEntry(T[] entries, string filename) {
        this.Filename = filename;
        this.entries = entries;
        isLoaded = true;
    }

    public T[] Get() {
        if (!isLoaded) {
            isLoaded = true;

            using StreamReader sr = new StreamReader(Filename);
            using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

            entries = csvReader.GetRecords<T>().ToArray();
        }

        return entries!;
    }

    public T? Get(int index) {
        if (!isLoaded) {
            using StreamReader sr = new StreamReader(Filename);
            using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

            return csvReader.GetRecords<T>().ElementAtOrDefault(1);
        }

        return entries.ElementAtOrDefault(index);
    }

    object IStorageEntry.Get() {
        return Get()!;
    }
}

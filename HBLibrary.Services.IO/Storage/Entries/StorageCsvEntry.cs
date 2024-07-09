using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageCsvEntry : IStorageEntry {
    private IEnumerable? entry;
    public string Filename { get; }

    public StorageEntryContentType ContentType => StorageEntryContentType.Csv;

    public Type? CurrentEntryType => entry?.GetType();

    internal StorageCsvEntry(string filename) {
        this.Filename = filename;
    }

    public object? Get(Type type) {
        if(!typeof(IEnumerable).IsAssignableFrom(type)) {
            throw new InvalidOperationException("Invalid type, needs to be enumerable.");
        }

        try {
            if (entry is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return null;
                }

                using StreamReader sr = new StreamReader(Filename);
                using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

                entry = csvReader.GetRecords(type).ToArray();
            }

            return entry;
        }
        catch {
            return null;
        }
    }

    public void Set(object value) {
        if(value is not IEnumerable) {
            throw new InvalidOperationException("Cannot set entry, object needs to be enumerable.");
        }

        entry = (IEnumerable)value;
    }

    public void Save(Type type) {
        if (entry is null) {
            throw new InvalidOperationException($"{nameof(entry)} is null.");
        }

        if (entry.GetType() != type) {
            throw new InvalidOperationException("Cannot save, entry does not equal given type.");
        }

        using StreamWriter sr = new StreamWriter(Filename);
        using CsvWriter csvWriter = new CsvWriter(sr, CultureInfo.InvariantCulture);

        csvWriter.WriteRecords(entry);
    }

    public void Save() {
        if (CurrentEntryType is null) {
            throw new InvalidOperationException($"{nameof(entry)} is null.");
        }

        Save(CurrentEntryType);
    }
}

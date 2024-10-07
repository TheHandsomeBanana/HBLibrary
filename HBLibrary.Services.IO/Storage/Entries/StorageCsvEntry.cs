using CsvHelper;
using HBLibrary.Services.IO.Storage.Settings;
using System.Collections;
using System.Globalization;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageCsvEntry : StorageEntry, IStorageEntry {
    internal StorageCsvEntry(string filename, StorageEntrySettings settings) : base(filename, StorageEntryContentType.Csv, settings) {
    }

    public object? Get(Type type) {
        if (!typeof(IEnumerable).IsAssignableFrom(type)) {
            throw new InvalidOperationException("Invalid type, needs to be enumerable.");
        }

        try {
            if (Value is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return null;
                }

                using StreamReader sr = new StreamReader(Filename);
                using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

                Value = csvReader.GetRecords(type).ToArray();
            }

            return Value;
        }
        catch {
            return null;
        }
    }

    public async Task<object?> GetAsync(Type type) {
        if (!typeof(IEnumerable).IsAssignableFrom(type)) {
            throw new InvalidOperationException("Invalid type, needs to be enumerable.");
        }

        try {
            if (Value is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return null;
                }

                using StreamReader sr = new StreamReader(Filename);
                using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

                var records = new List<object>();
                await foreach (var record in csvReader.GetRecordsAsync(type)) {
                    records.Add(record);
                }

                Value = records.ToArray();
            }

            return Value;
        }
        catch {
            return null;
        }
    }

    public override void Set(object value) {
        if (value is not IEnumerable) {
            throw new InvalidOperationException("Cannot set entry, object needs to be enumerable.");
        }

        Value = value;
    }

    public void Save() {
        if (Value is null) {
            throw new InvalidOperationException($"{nameof(Value)} is null.");
        }

        if (!typeof(IEnumerable).IsAssignableFrom(Value.GetType())) {
            throw new InvalidOperationException("Invalid type, needs to be enumerable.");
        }

        using StreamWriter sr = new StreamWriter(Filename);
        using CsvWriter csvWriter = new CsvWriter(sr, CultureInfo.InvariantCulture);

        csvWriter.WriteRecords((IEnumerable)Value);
    }

    public Task SaveAsync() {
        if (Value is null) {
            throw new InvalidOperationException($"{nameof(Value)} is null.");
        }

        if (!typeof(IEnumerable).IsAssignableFrom(Value.GetType())) {
            throw new InvalidOperationException("Invalid type, needs to be enumerable.");
        }

        using StreamWriter sr = new StreamWriter(Filename);
        using CsvWriter csvWriter = new CsvWriter(sr, CultureInfo.InvariantCulture);

        return csvWriter.WriteRecordsAsync((IEnumerable)Value);
    }

    public T? Get<T>() {
        if (!typeof(IEnumerable).IsAssignableFrom(typeof(T))) {
            throw new InvalidOperationException("Invalid type, needs to be enumerable.");
        }

        try {
            if (Value is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                using StreamReader sr = new StreamReader(Filename);
                using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

                Value = csvReader.GetRecords<T>().ToArray();
            }

            return (T?)Value;
        }
        catch {
            return default;
        }
    }

    public async Task<T?> GetAsync<T>() {
        if (!typeof(IEnumerable).IsAssignableFrom(typeof(T))) {
            throw new InvalidOperationException("Invalid type, needs to be enumerable.");
        }

        try {
            if (Value is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                using StreamReader sr = new StreamReader(Filename);
                using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

                var records = new List<T>();
                await foreach (var record in csvReader.GetRecordsAsync<T>()) {
                    records.Add(record);
                }

                Value = records.ToArray();
            }

            return (T?)Value;
        }
        catch {
            return default;
        }
    }

    public void Save<T>() {
        if (Value is null) {
            throw new InvalidOperationException($"{nameof(Value)} is null.");
        }

        if (Value is not T) {
            throw new InvalidOperationException("Cannot save, entry does not equal given type.");
        }

        using StreamWriter sr = new StreamWriter(Filename);
        using CsvWriter csvWriter = new CsvWriter(sr, CultureInfo.InvariantCulture);

        csvWriter.WriteRecords((IEnumerable)Value);
    }

    public Task SaveAsync<T>() {
        if (Value is null) {
            throw new InvalidOperationException($"{nameof(Value)} is null.");
        }

        if (Value is not T) {
            throw new InvalidOperationException("Cannot save, entry does not equal given type.");
        }

        using StreamWriter sr = new StreamWriter(Filename);
        using CsvWriter csvWriter = new CsvWriter(sr, CultureInfo.InvariantCulture);

        return csvWriter.WriteRecordsAsync((IEnumerable)Value);
    }

    protected override void OnLifetimeOver(object sender, TimeSpan fullTime) {
        Save();

        if (Value is IDisposable disposable) {
            disposable.Dispose();
        }
        else {
            Value = null;
        }
    }
}

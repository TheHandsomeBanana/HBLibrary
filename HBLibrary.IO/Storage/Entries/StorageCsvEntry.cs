using CsvHelper;
using HBLibrary.Interface.Core.ChangeTracker;
using HBLibrary.Interface.IO;
using HBLibrary.Interface.IO.Storage.Entries;
using HBLibrary.Interface.IO.Storage.Settings;
using System.Collections;
using System.Globalization;

namespace HBLibrary.IO.Storage.Entries;
internal class StorageCsvEntry : StorageEntry, IStorageEntry {

    // TODO: Implement cryptography
    internal StorageCsvEntry(string filename, StorageEntrySettings settings, IChangeTracker? changeTracker) 
        : base(filename, StorageEntryContentType.Csv, settings, changeTracker) {
    }

    public object? Get(Type type) {
        if (!typeof(IEnumerable).IsAssignableFrom(type)) {
            throw new InvalidOperationException("Invalid type, needs to be enumerable.");
        }

        lock (Lock) {
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
    }

    public async Task<object?> GetAsync(Type type) {
        if (!typeof(IEnumerable).IsAssignableFrom(type)) {
            throw new InvalidOperationException("Invalid type, needs to be enumerable.");
        }

        await Semaphore.WaitAsync();

        try {
            if (Value is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return null;
                }

                await Semaphore.WaitAsync();

                using StreamReader sr = new StreamReader(Filename);
                using CsvReader csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

                List<object> records = new List<object>();

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
        finally {
            Semaphore.Release();
        }
    }

    public override void Set(object value) {
        if (value is not IEnumerable) {
            throw new InvalidOperationException("Cannot set entry, object needs to be enumerable.");
        }

        Value = value;

        NotifyTrackableChanged(new TrackedChanges {
            Name = Filename,
            Value = Value
        });
    }

    public void Save() {
        lock (Lock) {
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
    }

    public async Task SaveAsync() {
        await Semaphore.WaitAsync();

        try {
            if (Value is null) {
                throw new InvalidOperationException($"{nameof(Value)} is null.");
            }

            if (!typeof(IEnumerable).IsAssignableFrom(Value.GetType())) {
                throw new InvalidOperationException("Invalid type, needs to be enumerable.");
            }


            using StreamWriter sr = new StreamWriter(Filename);
            using CsvWriter csvWriter = new CsvWriter(sr, CultureInfo.InvariantCulture);

            await csvWriter.WriteRecordsAsync((IEnumerable)Value);
        }
        finally {
            Semaphore.Release();
        }
    }

    public T? Get<T>() {
        if (!typeof(IEnumerable).IsAssignableFrom(typeof(T))) {
            throw new InvalidOperationException("Invalid type, needs to be enumerable.");
        }

        lock (Lock) {
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
    }

    public async Task<T?> GetAsync<T>() {
        if (!typeof(IEnumerable).IsAssignableFrom(typeof(T))) {
            throw new InvalidOperationException("Invalid type, needs to be enumerable.");
        }

        await Semaphore.WaitAsync();

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
        finally {
            Semaphore.Release();
        }
    }

    public void Save<T>() {
        lock (Lock) {
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
    }

    public async Task SaveAsync<T>() {
        await Semaphore.WaitAsync();

        try {

            if (Value is null) {
                throw new InvalidOperationException($"{nameof(Value)} is null.");
            }

            if (Value is not T) {
                throw new InvalidOperationException("Cannot save, entry does not equal given type.");
            }

            using StreamWriter sr = new StreamWriter(Filename);
            using CsvWriter csvWriter = new CsvWriter(sr, CultureInfo.InvariantCulture);

            await csvWriter.WriteRecordsAsync((IEnumerable)Value);
        }
        finally {
            Semaphore.Release();
        }
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

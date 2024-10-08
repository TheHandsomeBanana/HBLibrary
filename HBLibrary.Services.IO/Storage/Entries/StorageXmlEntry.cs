using HBLibrary.Services.IO.Storage.Settings;
using HBLibrary.Services.IO.Xml;

namespace HBLibrary.Services.IO.Storage.Entries;
public class StorageXmlEntry : StorageEntry, IStorageEntry {
    private readonly IXmlFileService xmlService;
    internal StorageXmlEntry(IXmlFileService xmlService, string filename, StorageEntrySettings settings) : base(filename, StorageEntryContentType.Xml, settings) {
        this.xmlService = xmlService;
    }

    public object? Get(Type type) {
        lock (Lock) {
            try {
                if (Value is null) {
                    if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                        return null;
                    }

                    Value = xmlService.ReadXml(type, file!);
                }

                return Value;
            }
            catch {
                return null;
            }
        }
    }

    public async Task<object?> GetAsync(Type type) {
        await Semaphore.WaitAsync();

        try {
            if (Value is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return null;
                }

                Value = await xmlService.ReadXmlAsync(type, file!);
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

    public void Save() {
        lock (Lock) {
            if (Value is null) {
                throw new InvalidOperationException(nameof(Value));
            }

            xmlService.WriteXml(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
        }
    }

    public async Task SaveAsync() {
        await Semaphore.WaitAsync();
        try {
            if (Value is null) {
                throw new InvalidOperationException(nameof(Value));
            }

            await xmlService.WriteXmlAsync(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
        }
        finally {
            Semaphore.Release();
        }
    }

    public T? Get<T>() {
        lock (Lock) {
            try {
                if (Value is null) {
                    if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                        return default;
                    }

                    Value = xmlService.ReadXml<T>(file!);
                }

                return (T?)Value;
            }
            catch {
                return default;
            }
        }
    }

    public async Task<T?> GetAsync<T>() {
        await Semaphore.WaitAsync();

        try {
            if (Value is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                Value = await xmlService.ReadXmlAsync<T>(file!);
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
                throw new InvalidOperationException(nameof(Value));
            }

            if (Value is not T tValue) {
                throw new InvalidOperationException("Cannot save, entry does not equal given type.");
            }

            xmlService.WriteXml(FileSnapshot.Create(Filename, true), tValue);
        }
    }

    public async Task SaveAsync<T>() {
        await Semaphore.WaitAsync();

        try {

        if (Value is null) {
            throw new InvalidOperationException(nameof(Value));
        }

        if (Value is not T tValue) {
            throw new InvalidOperationException("Cannot save, entry does not equal given type.");
        }

        await xmlService.WriteXmlAsync(FileSnapshot.Create(Filename, true), tValue);
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

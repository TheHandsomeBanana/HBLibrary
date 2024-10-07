using HBLibrary.Services.IO.Storage.Settings;
using HBLibrary.Services.IO.Xml;

namespace HBLibrary.Services.IO.Storage.Entries;
public class StorageXmlEntry : StorageEntry, IStorageEntry {
    private readonly IXmlFileService xmlService;
    internal StorageXmlEntry(IXmlFileService xmlService, string filename, StorageEntrySettings settings) : base(filename, StorageEntryContentType.Xml, settings) {
        this.xmlService = xmlService;
    }

    public object? Get(Type type) {
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
    
    public async Task<object?> GetAsync(Type type) {
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
    }

    public void Save() {
        if (Value is null) {
            throw new InvalidOperationException(nameof(Value));
        }

        xmlService.WriteXml(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
    }
    
    public Task SaveAsync() {
        if (Value is null) {
            throw new InvalidOperationException(nameof(Value));
        }

        return xmlService.WriteXmlAsync(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
    }

    public T? Get<T>() {
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
    
    public async Task<T?> GetAsync<T>() {
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
    }

    public void Save<T>() {
        if (Value is null) {
            throw new InvalidOperationException(nameof(Value));
        }

        if (Value is not T tValue) {
            throw new InvalidOperationException("Cannot save, entry does not equal given type.");
        }

        xmlService.WriteXml(FileSnapshot.Create(Filename, true), tValue);
    }
    
    public Task SaveAsync<T>() {
        if (Value is null) {
            throw new InvalidOperationException(nameof(Value));
        }

        if (Value is not T tValue) {
            throw new InvalidOperationException("Cannot save, entry does not equal given type.");
        }

        return xmlService.WriteXmlAsync(FileSnapshot.Create(Filename, true), tValue);
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

using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Settings;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageJsonEntry : StorageEntry, IStorageEntry {
    private readonly IJsonFileService jsonService;

    internal StorageJsonEntry(IJsonFileService jsonService, string filename, StorageEntrySettings settings)
        : base(filename, StorageEntryContentType.Json, settings) {
        this.jsonService = jsonService;
    }

    public object? Get(Type type) {
        try {
            if (Value is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                if (Settings.LifeTime!.Type != EntryLifetimeType.NoLifetime) {
                    Value = jsonService.ReadJson(type, file!);
                }
            }

            return Value;
        }
        catch {
            return default;
        }
    }

    public void Save(Type type) {
        if (Value is not null) {
            if (Value.GetType() != type) {
                throw new InvalidOperationException("Cannot save, entry does not equal given type.");
            }

            jsonService.WriteJson(type, FileSnapshot.Create(Filename, true), Value);
        }
    }

    public void Save() {
        if (CurrentEntryType is not null) {
            Save(CurrentEntryType);
        }
    }

    public T? Get<T>() {
        try {
            if (Value is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                if (Settings.LifeTime!.Type != EntryLifetimeType.NoLifetime) {
                    Value = jsonService.ReadJson<T>(file!);
                }
            }

            return (T?)Value;
        }
        catch {
            return default;
        }
    }

    public void Save<T>() {
        if (Value is not null) {
            if (Value is T tValue) {
                jsonService.WriteJson<T>(FileSnapshot.Create(Filename, true), tValue);
            }
            else {
                throw new InvalidOperationException("Cannot save, entry does not equal given type.");
            }
        }
    }

    protected override void OnLifetimeOver(object sender, TimeSpan fullTime) {
        Save();

        if (Value is IDisposable disposable) {
            disposable.Dispose();
        }

        Value = null;
    }
}

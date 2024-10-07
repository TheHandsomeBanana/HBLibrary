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

    public async Task<object?> GetAsync(Type type) {
        try {
            if (Value is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                if (Settings.LifeTime!.Type != EntryLifetimeType.NoLifetime) {
                    Value = await jsonService.ReadJsonAsync(type, file!);
                }
            }

            return Value;
        }
        catch {
            return default;
        }
    }



    public void Save(Type type) {

    }

    public void Save() {
        if (Value is not null) {
            jsonService.WriteJson(Value.GetType(), FileSnapshot.Create(Filename, true), Value);

        }
    }

    public Task SaveAsync() {
        if (Value is not null) {
            return jsonService.WriteJsonAsync(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
        }

        return Task.CompletedTask;
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

    public async Task<T?> GetAsync<T>() {
        try {
            if (Value is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                if (Settings.LifeTime!.Type != EntryLifetimeType.NoLifetime) {
                    Value = await jsonService.ReadJsonAsync<T>(file!);
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

    public Task SaveAsync<T>() {
        if (Value is not null) {
            if (Value is T tValue) {
                return jsonService.WriteJsonAsync<T>(FileSnapshot.Create(Filename, true), tValue);
            }
            else {
                throw new InvalidOperationException("Cannot save, entry does not equal given type.");
            }
        }

        return Task.CompletedTask;
    }

    protected override void OnLifetimeOver(object sender, TimeSpan fullTime) {
        Save();

        if (Value is IDisposable disposable) {
            disposable.Dispose();
        }

        Value = null;
    }
}

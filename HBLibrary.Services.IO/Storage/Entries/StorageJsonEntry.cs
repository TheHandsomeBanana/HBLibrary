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
        lock (Lock) {

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
    }

    public async Task<object?> GetAsync(Type type) {
        await Semaphore.WaitAsync();
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
        finally {
            Semaphore.Release();
        }
    }


    public void Save() {
        lock (Lock) {
            if (Value is not null) {
                jsonService.WriteJson(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
            }
        }
    }

    public async Task SaveAsync() {
        await Semaphore.WaitAsync();
        try {

            if (Value is not null) {
                await jsonService.WriteJsonAsync(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
            }
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
    }

    public async Task<T?> GetAsync<T>() {
        await Semaphore.WaitAsync();

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
        finally {
            Semaphore.Release();
        }
    }

    public void Save<T>() {
        lock (Lock) {
            if (Value is not null) {
                if (Value is T tValue) {
                    jsonService.WriteJson<T>(FileSnapshot.Create(Filename, true), tValue);
                }
                else {
                    throw new InvalidOperationException("Cannot save, entry does not equal given type.");
                }
            }
        }
    }

    public async Task SaveAsync<T>() {
        await Semaphore.WaitAsync();
        try {

            if (Value is not null) {
                if (Value is T tValue) {
                    await jsonService.WriteJsonAsync<T>(FileSnapshot.Create(Filename, true), tValue);
                }
                else {
                    throw new InvalidOperationException("Cannot save, entry does not equal given type.");
                }
            }
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

        Value = null;
    }
}

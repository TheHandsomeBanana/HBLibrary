using HBLibrary.DataStructures;
using HBLibrary.Interface.Core.ChangeTracker;
using HBLibrary.Interface.IO;
using HBLibrary.Interface.IO.Json;
using HBLibrary.Interface.IO.Storage.Entries;
using HBLibrary.Interface.IO.Storage.Settings;
using HBLibrary.Interface.Security;
using HBLibrary.Interface.Security.Keys;
using HBLibrary.Security;

namespace HBLibrary.IO.Storage.Entries;
internal class StorageJsonEntry : StorageEntry, IStorageEntry {
    private readonly IJsonFileService jsonService;

    internal StorageJsonEntry(IJsonFileService jsonService, string filename, StorageEntrySettings settings, IChangeTracker? changeTracker)
        : base(filename, StorageEntryContentType.Json, settings, changeTracker) {
        this.jsonService = jsonService;
    }

    public object? Get(Type type) {
        lock (Lock) {

            try {
                if (Settings.LifeTime!.Type == EntryLifetimeType.NoLifetime) {
                    // container.AddOrUpdate called and not saved yet
                    // Then Value is set but file is not written yet
                    if (Value is not null) {
                        return Value;
                    }

                    if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                        return default;
                    }

                    return GetInternal(type, file);
                }
                else {
                    if (Value is null) {

                        if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                            return default;
                        }

                        Value = GetInternal(type, file);
                    }

                    return Value;
                }
            }
            catch {
                return default;
            }
        }
    }

    private object? GetInternal(Type type, FileSnapshot file) {
        object? value;

        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();

            IKey key = keyResult.GetValueOrThrow();

            value = jsonService.DecryptJson(type, file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            value = jsonService.ReadJson(type, file);
        }

        if (value is INotifyTrackableChanged notifyTrackableChanged) {
            ChangeTracker?.Track(notifyTrackableChanged);
        }

        return value;
    }

    public async Task<object?> GetAsync(Type type) {
        await Semaphore.WaitAsync();
        try {
            if (Settings.LifeTime!.Type == EntryLifetimeType.NoLifetime) {
                // container.AddOrUpdate called and not saved yet
                // Then Value is set but file is not written yet
                if (Value is not null) {
                    return Value;
                }

                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                return await GetAsyncInternal(type, file);
            }
            else {
                if (Value is null) {
                    if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                        return default;
                    }

                    Value = await GetAsyncInternal(type, file);
                }

                return Value;
            }
        }
        catch {
            return default;
        }
        finally {
            Semaphore.Release();
        }
    }

    private async Task<object?> GetAsyncInternal(Type type, FileSnapshot file) {
        object? value;
        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = await Settings.ContainerCryptography!.GetEntryKeyAsync.Invoke();

            IKey key = keyResult.GetValueOrThrow();

            value = jsonService.DecryptJson(type, file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            value = await jsonService.ReadJsonAsync(type, file);
        }

        if (value is INotifyTrackableChanged notifyTrackableChanged) {
            ChangeTracker?.Track(notifyTrackableChanged);
        }

        return value;
    }

    public void Save() {
        lock (Lock) {
            if (Value is not null) {
                if (Settings.EncryptionEnabled) {
                    Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();

                    IKey key = keyResult.GetValueOrThrow();

                    jsonService.EncryptJson(Value.GetType(), FileSnapshot.Create(Filename, true), Value, new Cryptographer(), new CryptographyInput {
                        Key = key,
                        Mode = Settings.ContainerCryptography.CryptographyMode
                    });
                }
                else {
                    jsonService.WriteJson(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
                }

                if (Value is INotifyTrackableChanged notifyTrackableChanged) {
                    ChangeTracker?.SaveChanges(notifyTrackableChanged);
                }
            }
        }
    }

    public async Task SaveAsync() {
        await Semaphore.WaitAsync();
        try {

            if (Value is not null) {
                if (Settings.EncryptionEnabled) {
                    Result<IKey> keyResult = await Settings.ContainerCryptography!.GetEntryKeyAsync.Invoke();

                    IKey key = keyResult.GetValueOrThrow();

                    jsonService.EncryptJson(Value.GetType(), FileSnapshot.Create(Filename, true), Value, new Cryptographer(), new CryptographyInput {
                        Key = key,
                        Mode = Settings.ContainerCryptography.CryptographyMode
                    });
                }
                else {
                    await jsonService.WriteJsonAsync(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
                }

                if (Value is INotifyTrackableChanged notifyTrackableChanged) {
                    ChangeTracker?.SaveChanges(notifyTrackableChanged);
                }
            }
        }
        finally {
            Semaphore.Release();
        }
    }

    public T? Get<T>() {
        lock (Lock) {
            try {
                if (Settings.LifeTime!.Type == EntryLifetimeType.NoLifetime) {
                    // container.AddOrUpdate called and not saved yet
                    // Then Value is set but file is not written yet
                    if (Value is not null) {
                        return (T?)Value;
                    }

                    if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                        return default;
                    }

                    return GetInternal<T>(file);
                }
                else {
                    if (Value is null) {
                        if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                            return default;
                        }

                        Value = GetInternal<T>(file);
                    }

                    return (T?)Value;
                }
            }
            catch {
                return default;
            }
        }
    }

    private T? GetInternal<T>(FileSnapshot file) {
        T? value;

        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();

            IKey key = keyResult.GetValueOrThrow();

            value = jsonService.DecryptJson<T>(file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            value = jsonService.ReadJson<T>(file);
        }

        if (value is INotifyTrackableChanged notifyTrackableChanged) {
            ChangeTracker?.Track(notifyTrackableChanged);
        }

        return value;
    }

    public async Task<T?> GetAsync<T>() {
        await Semaphore.WaitAsync();

        try {
            if (Settings.LifeTime!.Type == EntryLifetimeType.NoLifetime) {
                // container.AddOrUpdate called and not saved yet
                // Then Value is set but file is not written yet
                if (Value is not null) {
                    return (T?)Value;
                }

                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                return await GetAsyncInternal<T>(file);
            }
            else {
                if (Value is null) {
                    if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                        return default;
                    }

                    Value = await GetAsyncInternal<T>(file);
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

    private async Task<T?> GetAsyncInternal<T>(FileSnapshot file) {
        T? value;

        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = await Settings.ContainerCryptography!.GetEntryKeyAsync.Invoke();

            IKey key = keyResult.GetValueOrThrow();
            value = jsonService.DecryptJson<T>(file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            value = await jsonService.ReadJsonAsync<T>(file);
        }

        if (value is INotifyTrackableChanged notifyTrackableChanged) {
            ChangeTracker?.Track(notifyTrackableChanged);
        }

        return value;
    }

    public void Save<T>() {
        lock (Lock) {
            if (Value is T tValue) {
                if (Settings.EncryptionEnabled) {
                    Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();

                    IKey key = keyResult.GetValueOrThrow();

                    jsonService.EncryptJson(FileSnapshot.Create(Filename, true), tValue, new Cryptographer(), new CryptographyInput {
                        Key = key,
                        Mode = Settings.ContainerCryptography.CryptographyMode
                    });
                }
                else {
                    jsonService.WriteJson(FileSnapshot.Create(Filename, true), tValue);
                }

                if (Value is INotifyTrackableChanged notifyTrackableChanged) {
                    ChangeTracker?.SaveChanges(notifyTrackableChanged);
                }
            }
            else {
                throw new InvalidOperationException("Cannot save, entry does not equal given type.");
            }
        }
    }

    public async Task SaveAsync<T>() {
        await Semaphore.WaitAsync();
        try {
            if (Value is T tValue) {
                if (Settings.EncryptionEnabled) {
                    Result<IKey> keyResult = await Settings.ContainerCryptography!.GetEntryKeyAsync.Invoke();

                    IKey key = keyResult.GetValueOrThrow();

                    jsonService.EncryptJson(FileSnapshot.Create(Filename, true), tValue, new Cryptographer(), new CryptographyInput {
                        Key = key,
                        Mode = Settings.ContainerCryptography.CryptographyMode
                    });
                }
                else {
                    await jsonService.WriteJsonAsync(FileSnapshot.Create(Filename, true), tValue);
                }

                if (Value is INotifyTrackableChanged notifyTrackableChanged) {
                    ChangeTracker?.SaveChanges(notifyTrackableChanged);
                }
            }
            else {
                throw new InvalidOperationException("Cannot save, entry does not equal given type.");
            }
        }
        finally {
            Semaphore.Release();
        }
    }

    protected override void OnLifetimeOver(object sender, TimeSpan fullTime) {
        Save();

        if(Value is INotifyTrackableChanged notifyTrackableChanged) {
            ChangeTracker?.Untrack(notifyTrackableChanged);
        }

        if (Value is IDisposable disposable) {
            disposable.Dispose();
        }

        Value = null;
    }
}
